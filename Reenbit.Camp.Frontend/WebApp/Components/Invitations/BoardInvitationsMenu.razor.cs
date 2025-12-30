using Domain.Models.Boards;
using Domain.Models.Invitations;
using Domain.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Services.Abstractions.Services;
using Telerik.Blazor.Components;

namespace WebApp.Components.Invitations;

public partial class BoardInvitationsMenu : ComponentBase
{
    private TelerikPopover? PopoverRef { get; set; }
    
    [Inject] 
    public IInvitationsService InvitationsService { get; set; } = default!;
    
    private bool IsLoading;
    private bool IsOpen;

    private List<InvitationModel> Invitations = new();
    
    private async Task Toggle()
    {
        IsOpen = !IsOpen;

        if (IsOpen)
        {
            IsLoading = true;
            PopoverRef?.Show();
            await LoadInvitations();
        }
        else
        {
            PopoverRef?.Hide();
            Invitations.Clear();
        }

        IsLoading = false;
        PopoverRef?.Refresh();
    }

    private async Task LoadInvitations()
    {
        var result = await InvitationsService.GetByUserAsync();

        if (result.IsSuccess)
        {
            Invitations = result.Value;
        }
    }

    private async Task Accept(int invitationId)
    {
        var result =  await InvitationsService.AcceptInvitationAsync(invitationId);
        
        if (result.IsSuccess)
        {
            Invitations.RemoveAll(i => i.Id == invitationId);
        }
        
        PopoverRef?.Refresh();
    }

    private async Task Decline(int invitationId)
    {
        var result = await InvitationsService.DeclineInvitationAsync(invitationId);
        
        if (result.IsSuccess)
        {
            Invitations.RemoveAll(i => i.Id == invitationId);
        }
        
        PopoverRef?.Refresh();
    }
}