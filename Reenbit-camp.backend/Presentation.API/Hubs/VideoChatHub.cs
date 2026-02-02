using Domain.Constants.HubConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Presentation.API.Hubs;

[Authorize]
public class VideoChatHub : Hub
{
    public async Task JoinBoard(int boardId)
    {
        var roomId = GetRoomGroupName(boardId);
        var userId = Context.UserIdentifier!;
        
        await Groups.AddToGroupAsync(
            Context.ConnectionId, 
            GetRoomGroupName(boardId));
        
        await Clients.OthersInGroup(roomId)
            .SendAsync(
                VideoChatHubConstants.UserJoined, 
                userId);
    }

    public async Task LeaveBoard(int boardId)
    {
        var roomId = GetRoomGroupName(boardId);
        var userId = Context.UserIdentifier!;

        await Clients.OthersInGroup(roomId)
            .SendAsync(
                VideoChatHubConstants.UserLeft, 
                userId);
        
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId, 
            roomId);
    }

    public async Task SendOffer(
        string targetUserId, 
        string offer,
        bool isVideoActive, 
        bool isAudioActive)
    {
        await Clients.User(targetUserId)
            .SendAsync(
                VideoChatHubConstants.ReceiveOffer, 
                Context.UserIdentifier, 
                offer,
                isVideoActive,
                isAudioActive);
    }

    public async Task SendAnswer(string targetUserId, string answer)
    {
        await Clients.User(targetUserId)
            .SendAsync(
                VideoChatHubConstants.ReceiveAnswer, 
                Context.UserIdentifier, 
                answer);
    }

    public async Task SendIce(string targetUserId, string candidate)
    {
        await Clients.User(targetUserId)
            .SendAsync(
                VideoChatHubConstants.ReceiveIce, 
                Context.UserIdentifier, 
                candidate);
    }
    
    public async Task ToggleVideo(bool status, int boardId)
    {
        var roomId = GetRoomGroupName(boardId);

        await Clients.OthersInGroup(roomId)
            .SendAsync(
                VideoChatHubConstants.ChangeVideoStatus, 
                Context.UserIdentifier, 
                status);
    }
    
    public async Task ToggleAudio(bool status, int boardId)
    {
        var roomId = GetRoomGroupName(boardId);
        
        await Clients.OthersInGroup(roomId)
            .SendAsync(
                VideoChatHubConstants.ChangeAudioStatus, 
                Context.UserIdentifier, 
                status);
    }
    
    public async Task StopScreenShare(int boardId)
    {
        var roomId = GetRoomGroupName(boardId);
        
        await Clients.OthersInGroup(roomId)
            .SendAsync(VideoChatHubConstants.ScreenShareStopped);
    }
    
    public async Task SendScreenOffer(string targetUserId, string offer)
    {
        await Clients.User(targetUserId)
            .SendAsync(
                VideoChatHubConstants.ReceiveScreenOffer, 
                Context.UserIdentifier, 
                offer);
    }
    
    public async Task SendScreenAnswer(string targetUserId, string answer)
    {
        await Clients.User(targetUserId)
            .SendAsync(
                VideoChatHubConstants.ReceiveScreenAnswer, 
                Context.UserIdentifier, 
                answer);
    }
    
    public async Task SendScreenIce(string targetUserId, string candidate)
    {
        await Clients.User(targetUserId)
            .SendAsync( 
                VideoChatHubConstants.ReceiveScreenIce, 
                Context.UserIdentifier, 
                candidate);
    }

    public static string GetRoomGroupName(int boardId)
    {
        return $"room_{boardId}";
    }
}