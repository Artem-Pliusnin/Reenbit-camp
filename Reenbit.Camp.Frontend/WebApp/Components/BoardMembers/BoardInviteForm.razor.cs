using System.Timers;
using Domain.Enums;
using Domain.Models.BoardMembers;
using Domain.Models.Invitations;
using Domain.Models.Users;
using Domain.Requests.Invitations;
using Domain.Requests.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services.Abstractions.Services;


namespace WebApp.Components.BoardMembers;

public partial class BoardInviteForm : ComponentBase
{
    [Parameter, EditorRequired]
    public int BoardId { get; set; }
    
    [CascadingParameter(Name="CurrentUser")]
    private BoardMemberModel CurrentUser { get; set; }
    
    [Inject] 
    public IUsersService UsersService { get; set; } = default!;
    
    [Inject] 
    public IInvitationsService InvitationsService { get; set; } = default!;

    private string? InputUser = string.Empty;
    private List<UserModel> Suggestions = new();
    
    private System.Timers.Timer aTimer = default!;

    private bool IsInvitationsLoading;

    private List<InvitationModel> PendingInvitations = new();
    
    protected override async Task OnInitializedAsync()
    {
        await LoadInvitations();
        
        aTimer = new System.Timers.Timer(300);
        aTimer.Elapsed += OnTimerElapsed;
        aTimer.AutoReset = false;
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
            new CreateInvitationRequest(BoardId, userId));

        if (result.IsSuccess)
        {
            await LoadInvitations();
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
        var result = await InvitationsService.DeleteAsync(invitation.Id);

        if (result.IsSuccess)
        {
            PendingInvitations.Remove(invitation);
            StateHasChanged();
        }
    }
}