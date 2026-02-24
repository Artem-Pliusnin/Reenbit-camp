using System.Timers;
using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.Enums;
using Domain.Models.BoardMembers;
using Domain.Models.Invitations;
using Domain.Models.Users;
using Domain.Requests.Invitations;
using Domain.Requests.Users;
using Domain.Responses.BoardMembers;
using Domain.Responses.Invitations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;


namespace WebApp.Components.BoardMembers;

public partial class BoardInviteForm : ComponentBase, IDisposable
{
    [Parameter, EditorRequired]
    public int BoardId { get; set; }
    
    [CascadingParameter(Name="CurrentUser")]
    private BoardMemberModel CurrentUser { get; set; }
    
    [Inject] 
    public IUsersService UsersService { get; set; } = default!;
    
    [Inject] 
    public IInvitationsService InvitationsService { get; set; } = default!;
    
    [Inject] 
    public IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    private List<IDisposable> Subscriptions = new();

    private string? InputUser = string.Empty;
    private List<UserModel> Suggestions = new();
    
    private System.Timers.Timer aTimer = default!;

    private bool IsInvitationsLoading;

    private List<InvitationModel> PendingInvitations = new();
    
    protected override async Task OnInitializedAsync()
    {
        IsInvitationsLoading = true;
        
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
        
        await LoadInvitations();
        
        aTimer = new System.Timers.Timer(300);
        aTimer.Elapsed += OnTimerElapsed;
        aTimer.AutoReset = false;
        
        Subscriptions.Add(HomeHubConnection
            .On<InvitationDto>(SubscribeHomeHubConstants.AddInvitation, AddInvitation));
        
        Subscriptions.Add(HomeHubConnection
            .On<int>(SubscribeHomeHubConstants.DeleteInvitation, DeleteInvitation));
        
        Subscriptions.Add(HomeHubConnection
            .On<UpdatedCardMemberRoleDto>(SubscribeHomeHubConstants.UpdateMemberRole, OnUpdateMemberRole));
    }

    private void ResetTimer(KeyboardEventArgs e)
    {
        aTimer.Stop();
        aTimer.Start();
    }
    
    private async void OnTimerElapsed(Object? source, ElapsedEventArgs e)
    {
        await SearchUsers();
    }
    
    private async Task SearchUsers()
    {
        var result = await UsersService.GetInvitationSuggestionsAsync(
            new GetInviteSuggestionRequest(BoardId, InputUser, 5));
        
        if (result.IsSuccess)
        {
            Suggestions = result.Value;
            await InvokeAsync(StateHasChanged);
        }
    }
    
    private async Task Invite(int userId)
    {
        var result = await InvitationsService.CreateAsync(
            BoardId,
            new CreateInvitationRequest(BoardId, userId));

        if (result.IsSuccess)
        {
            PendingInvitations.Add(result.Value);
            
            var inviattionDto = Mapper.Map<InvitationDto>(result.Value);
            
            await HomeHubConnection
                .SendAsync(
                    SendHomeHubConstants.AddInvitation, 
                    inviattionDto, 
                    BoardId);
        }
        
        Suggestions.RemoveAll(u => u.Id == userId);
    }

    private async Task LoadInvitations()
    {
        IsInvitationsLoading = true;
        
        var result = await InvitationsService.GetByBoardAsync(BoardId);
        if (result.IsSuccess)
        {
            PendingInvitations = result.Value;
        }
        
        IsInvitationsLoading = false;
    }
    
    private bool CanRemove()
    {
        return CurrentUser.Role == BoardRole.Owner ||
               CurrentUser.Role == BoardRole.Admin;
    }
    
    private async Task RemoveInvitation(InvitationModel invitation)
    {
        var result = await InvitationsService.DeleteAsync(BoardId, invitation.Id);

        if (result.IsSuccess)
        {
            PendingInvitations.Remove(invitation);
            StateHasChanged();
        }
    }

    private void AddInvitation(InvitationDto invitation)
    {
        if (!PendingInvitations.Any(i => i.Id == invitation.Id))
        {
            var invitationModel = Mapper.Map<InvitationModel>(invitation);
            PendingInvitations.Add(invitationModel);
            Suggestions.RemoveAll(u => u.Id == invitation.InvitedUser.Id);
            StateHasChanged();
        }
    }
    
    private void DeleteInvitation(int invitationId)
    {
        if(PendingInvitations.RemoveAll(i => i.Id == invitationId) > 0){
            StateHasChanged();
        }
    }
    
    private void OnUpdateMemberRole(UpdatedCardMemberRoleDto dto)
    {
        if (CurrentUser.Id == dto.MemberId)
        {
            CurrentUser.Role = (BoardRole)dto.RoleId;
            StateHasChanged();
        }
    }

    public void Dispose()
    {
        foreach (var subscription in Subscriptions)
        {
            subscription.Dispose();
        }
    }
}