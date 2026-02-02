using Domain.Constants.HubConstants;
using Domain.Models.VideoChatModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Services.Abstractions.Services;
using Services.HubServices;

namespace WebApp.Pages;

public partial class VideoChatPage : ComponentBase, IAsyncDisposable
{
    [Parameter]
    public int BoardId { get; set; }
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    
    [Inject]
    public IBoardMembersService BoardMembersService { get; set; } = default!;
    
    [Inject]
    public IUsersService UsersService { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection VideoChatHubConnection;
    
    private List<IDisposable> Subscriptions = new();
    
    private MeetingMemberModel CurrentUser;
    
    private Dictionary<string, MeetingMemberModel> ConnectedUsers = new();
    
    [Inject]
    public IJSRuntime JsRuntime { get; set; } = default!;
    
    private IJSObjectReference? _localStream;
    
    private DotNetObjectReference<VideoChatPage>? _pageReference;
    
    private Dictionary<string, IJSObjectReference> _peers = new();
    
    private Dictionary<string, IJSObjectReference> _receiverScreenPeers = new();
    
    private IJSObjectReference _screenPeer;
    
    private IJSObjectReference? _screenStream;
    
    private bool IsScreenSharing = false;
    
    private string? ScreenOwnerId;

    private bool IsLoading;
    
    private string ScreenOwnerName
    {
        get
        {
            if (ScreenOwnerId is not null 
                && ConnectedUsers.TryGetValue(ScreenOwnerId, out var screenOwner))
            {
                return screenOwner.User.UserName;
            }
            return string.Empty;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        if (!await TryLoadCurrentUser())
        {
            return;
        }
        
        _pageReference = DotNetObjectReference.Create(this);
        
        await HubConnectionManager.StartAsync(HubType.VideoChatHub);
        
        VideoChatHubConnection = HubConnectionManager.Get(HubType.VideoChatHub);
        
        RegisterHubHandlers();
        
        await InitLocalMedia();
        
        await VideoChatHubConnection
            .SendAsync(SendVideoChatHubConstants.JoinBoard, BoardId);
        
        IsLoading = false;
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!IsLoading && _localStream != null)
        {
            await JsRuntime.InvokeVoidAsync(
                "webrtc.displayLocalMedia",
                _localStream
            );
        }
    }
    
    private async Task InitLocalMedia()
    {
        _localStream = await JsRuntime.InvokeAsync<IJSObjectReference>(
            "webrtc.getUserMedia");
    }
    
    private async Task ToggleVideo()
    {
        CurrentUser.IsVideoActive = !CurrentUser.IsVideoActive;

        if (_localStream != null)
        {
            await JsRuntime.InvokeVoidAsync(
                "webrtc.toggleLocalVideo", 
                _localStream, 
                CurrentUser.IsVideoActive);
        }
        
        await VideoChatHubConnection
            .SendAsync(
                SendVideoChatHubConstants.ToggleVideo, 
                CurrentUser.IsVideoActive,
                BoardId);
        
        StateHasChanged();
    }

    private async Task ToggleAudio()
    {
        CurrentUser.IsAudioActive = !CurrentUser.IsAudioActive;

        if (_localStream != null)
        {
            await JsRuntime.InvokeVoidAsync(
                "webrtc.toggleLocalAudio", 
                _localStream, CurrentUser.IsAudioActive);
        }
         
        await VideoChatHubConnection
            .SendAsync(
                SendVideoChatHubConstants.ToggleAudio, 
                CurrentUser.IsAudioActive,
                BoardId);
        
        StateHasChanged();
    }
    
    private async Task ToggleScreenShare()
    {
        if (ScreenOwnerId != null && 
            ScreenOwnerId != CurrentUser.User.Id.ToString())
        {
            return;
        }

        if (IsScreenSharing)
        {
            await StopScreenShare();
            return;
        }

        try
        {
            _screenStream = await JsRuntime
                .InvokeAsync<IJSObjectReference>(
                    "webrtc.getDisplayMedia", 
                    _pageReference);
            
            IsScreenSharing = true;
            ScreenOwnerId = CurrentUser.User.Id.ToString();

            foreach (var user in ConnectedUsers)
            {
                if (user.Key == CurrentUser.User.Id.ToString())
                {
                    continue;
                }
                
                await SendScreenOfferToUser(user.Key);
            }
            
            StateHasChanged();
        }
        catch (JSException ex)
        {
            await StopScreenShare();
        }
    }


    [JSInvokable]
    public async Task StopScreenShare()
    {
        if (!IsScreenSharing)
        {
            return;
        }

        IsScreenSharing = false;
        ScreenOwnerId = null;

        if (_screenStream != null)
        {
            await JsRuntime.InvokeVoidAsync("webrtc.stopStream", _screenStream);
            await JsRuntime.InvokeVoidAsync("webrtc.removeScreenVideo");
            _screenStream = null;
        }

        foreach (var peer in _receiverScreenPeers)
        {
            await JsRuntime.InvokeVoidAsync("webrtc.closePeer", peer.Value);
            _receiverScreenPeers.Remove(peer.Key);
        }

        await VideoChatHubConnection
            .SendAsync(SendVideoChatHubConstants.StopScreenShare, BoardId);
        
        StateHasChanged();
    }


    private async Task LeaveMeeting()
    {
        if (IsScreenSharing)
        {
            await StopScreenShare();
        }
        
        await VideoChatHubConnection
            .SendAsync(SendVideoChatHubConstants.LeaveBoard, BoardId);

        NavigationManager.NavigateTo($"/board/{BoardId}");
    }

    private async Task OnUserJoined(string userId)
    {
        if (!await AddConnectedUser(userId, true, true))
        {
            return;
        }
        
        var newPeerConnection = await JsRuntime.InvokeAsync<IJSObjectReference>(
            "webrtc.createPeerConnection",
            _localStream,
            _pageReference,
            userId
        );

        _peers[userId] = newPeerConnection;

        var offer = await JsRuntime
            .InvokeAsync<string>(
                "webrtc.createOffer", 
                newPeerConnection);

        await VideoChatHubConnection.SendAsync(
            SendVideoChatHubConstants.SendOffer,
            userId,
            offer,
            CurrentUser.IsVideoActive,
            CurrentUser.IsAudioActive
        );

        if (IsScreenSharing)
        {
            await SendScreenOfferToUser(userId);
        }

        await InvokeAsync(StateHasChanged);
    }
    
    private async Task OnReceiveOffer(
        string fromUserId, 
        string offer, 
        bool isVideoActive, 
        bool isAudioActive)
    {
        if (!await AddConnectedUser(fromUserId, isVideoActive, isAudioActive))
        {
            return;
        }

        var newPeerConnection = await JsRuntime.InvokeAsync<IJSObjectReference>(
            "webrtc.createPeerConnection",
            _localStream,
            _pageReference,
            fromUserId
        );

        _peers[fromUserId] = newPeerConnection;

        await JsRuntime.InvokeVoidAsync(
            "webrtc.setRemoteDescription", 
            newPeerConnection, 
            offer);

        var answer = await JsRuntime.InvokeAsync<string>(
            "webrtc.createAnswer", 
            newPeerConnection);

        await VideoChatHubConnection.SendAsync(
            SendVideoChatHubConstants.SendAnswer,
            fromUserId,
            answer
        );
    }
    
    private async Task OnReceiveAnswer(string fromUserId, string answer)
    {
        if (_peers.TryGetValue(fromUserId, out var peerConnection))
        {
            await JsRuntime.InvokeVoidAsync(
                "webrtc.setRemoteDescription", 
                peerConnection, 
                answer);
            
            StateHasChanged();
        }
    }

    [JSInvokable]
    public async Task SendIceCandidate(string targetUserId, string candidate)
    {
        await VideoChatHubConnection.SendAsync(
            SendVideoChatHubConstants.SendIce,
            targetUserId,
            candidate
        );
    }
    
    private async Task OnReceiveIce(string fromUserId, string candidate)
    {
        if (_peers.TryGetValue(fromUserId, out var peerConnection))
        {
            await JsRuntime.InvokeVoidAsync(
                "webrtc.addIceCandidate", 
                peerConnection, 
                candidate);
        }
    }

    private async Task OnUserLeft(string userId)
    {
        ConnectedUsers.Remove(userId);
        
        if (_peers.TryGetValue(userId, out var peerConnection))
        {
            await JsRuntime.InvokeVoidAsync(
                "webrtc.removeUser", 
                peerConnection, 
                userId);
            
            _peers.Remove(userId);
            StateHasChanged();
        }
    }

    private async Task OnUserChangedVideoStatus(string userId, bool status)
    {
        if (ConnectedUsers.TryGetValue(userId, out var memberModel))
        {
            memberModel.IsVideoActive = status;
            
            await JsRuntime.InvokeVoidAsync(
                "webrtc.toggleMemberVideo", 
                status, 
                userId);
            
            StateHasChanged();
        }
    }
    
    private void OnUserChangedAudioStatus(string userId, bool status)
    {
        if (ConnectedUsers.TryGetValue(userId, out var memberModel))
        {
            memberModel.IsAudioActive = status;
            StateHasChanged();
        }
    }
    
    private async Task OnScreenShareStopped()
    {
        if (_screenPeer != null)
        {
            await JsRuntime.InvokeVoidAsync("webrtc.closePeer", _screenPeer);
            _screenPeer = null;
        }
        
        await JsRuntime.InvokeVoidAsync("webrtc.removeScreenVideo");
        
        ScreenOwnerId = null;
        StateHasChanged();
    }

    private async Task SendScreenOfferToUser(string userId)
    {
        var peer = await JsRuntime.InvokeAsync<IJSObjectReference>(
            "webrtc.createScreenPeer",
            _screenStream,
            _pageReference,
            userId
        );

        _receiverScreenPeers[userId] = peer;

        var offer = await JsRuntime.InvokeAsync<string>(
            "webrtc.createOffer",
            peer
        );

        await VideoChatHubConnection.SendAsync(
            SendVideoChatHubConstants.SendScreenOffer,
            userId,
            offer
        );
    }
    
    private async void OnReceiveScreenOffer(string fromUserId, string offer)
    {
        if (ScreenOwnerId is null)
        {
            ScreenOwnerId = fromUserId;
            StateHasChanged();
        }
        
        _screenPeer = await JsRuntime.InvokeAsync<IJSObjectReference>(
            "webrtc.createScreenReceiverPeer",
            _pageReference,
            fromUserId
        );
        
        await JsRuntime.InvokeVoidAsync(
            "webrtc.setRemoteDescription", 
            _screenPeer, 
            offer);
        
        var answer = await JsRuntime.InvokeAsync<string>(
            "webrtc.createAnswer", 
            _screenPeer);
        
        await VideoChatHubConnection.SendAsync(
            SendVideoChatHubConstants.SendScreenAnswer,
            fromUserId,
            answer
        );
    }
    
    private async Task OnReceiveScreenAnswer(string userId, string answer)
    {
        if (_receiverScreenPeers.TryGetValue(userId, out var peer))
        {
            await JsRuntime.InvokeVoidAsync(
                "webrtc.setRemoteDescription",
                peer,
                answer
            );
        }
    }
    
    [JSInvokable]
    public async Task SendScreenIceCandidate(string targetUserId, string candidate)
    {
        await VideoChatHubConnection.SendAsync(
            SendVideoChatHubConstants.SendScreenIce,
            targetUserId,
            candidate
        );
    }
    
    private async Task OnReceiveScreenIce(string fromUserId, string candidate)
    {
        if (_receiverScreenPeers.TryGetValue(fromUserId, out var peer))
        {
            await JsRuntime.InvokeVoidAsync(
                "webrtc.addIceCandidate", 
                peer, 
                candidate);
        }
    }

    private async Task<bool> TryLoadCurrentUser()
    {
        var result = await BoardMembersService.GetCurrentAsync(BoardId);
        if (result.IsSuccess)
        {
            CurrentUser = new MeetingMemberModel()
            {
                User = result.Value.User,
                IsVideoActive = true,
                IsAudioActive = true
            };;
            return true;
        }
        
        NavigationManager.NavigateTo($"/");
        return false;
    }
    
    private void RegisterHubHandlers()
    {
        Subscriptions.Add(VideoChatHubConnection
            .On<string>(SubscribeVideoChatHubConstants.UserJoined, OnUserJoined));

        Subscriptions.Add(VideoChatHubConnection
            .On<string, string, bool, bool>(SubscribeVideoChatHubConstants.ReceiveOffer, OnReceiveOffer));

        Subscriptions.Add(VideoChatHubConnection
            .On<string, string>(SubscribeVideoChatHubConstants.ReceiveAnswer, OnReceiveAnswer));

        Subscriptions.Add(VideoChatHubConnection
            .On<string, string>(SubscribeVideoChatHubConstants.ReceiveIce, OnReceiveIce));
        
        Subscriptions.Add(VideoChatHubConnection
            .On<string>(SubscribeVideoChatHubConstants.UserLeft, OnUserLeft));
        
        Subscriptions.Add(VideoChatHubConnection
            .On<string, bool>(SubscribeVideoChatHubConstants.ChangeVideoStatus, OnUserChangedVideoStatus));
        
        Subscriptions.Add(VideoChatHubConnection
            .On<string, bool>(SubscribeVideoChatHubConstants.ChangeAudioStatus, OnUserChangedAudioStatus));
        
        Subscriptions.Add(VideoChatHubConnection
            .On(SubscribeVideoChatHubConstants.ScreenShareStopped, OnScreenShareStopped));
        
        Subscriptions.Add(VideoChatHubConnection
            .On<string, string>(SubscribeVideoChatHubConstants.ReceiveScreenOffer, OnReceiveScreenOffer));
        
        Subscriptions.Add(VideoChatHubConnection
            .On<string, string>(SubscribeVideoChatHubConstants.ReceiveScreenAnswer, OnReceiveScreenAnswer));
        
        Subscriptions.Add(VideoChatHubConnection
            .On<string, string>(SubscribeVideoChatHubConstants.ReceiveScreenIce, OnReceiveScreenIce));
        
    }

    private async Task<bool> AddConnectedUser(
        string userId, 
        bool isVideoActive, 
        bool isAudioActive) 
    {
        if (int.TryParse(userId, out int id))
        {
            var result = await UsersService.GetUserInfoAsync(id);

            if (result.IsSuccess)
            {
                ConnectedUsers[userId] = new MeetingMemberModel()
                {
                    User = result.Value,
                    IsVideoActive = isVideoActive,
                    IsAudioActive = isAudioActive
                };
                
                StateHasChanged();
                return true;
            }
            
            return false;
        }
        
        return false;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var peer in _peers.Values)
        {
            await JsRuntime.InvokeVoidAsync("webrtc.closePeer", peer);
        }
        _peers.Clear();
        
        if (_localStream != null)
        {
            await JsRuntime.InvokeVoidAsync("webrtc.stopStream", _localStream);
        }
        
        if (VideoChatHubConnection != null)
        {
            await LeaveMeeting();
        }
        
        foreach (var subscription in Subscriptions)
        {
            subscription.Dispose();
        }
        
        _pageReference?.Dispose();
    }
}