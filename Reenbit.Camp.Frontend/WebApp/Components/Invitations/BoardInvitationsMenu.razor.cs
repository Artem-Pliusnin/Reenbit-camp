using AutoMapper;
using Domain.Models.Boards;
using Domain.Models.Invitations;
using Domain.Models.Users;
using Domain.Responses.Invitations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;

namespace WebApp.Components.Invitations;

public partial class BoardInvitationsMenu : ComponentBase
{
    private TelerikPopover? PopoverRef { get; set; }

    [Inject] 
    public IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public IInvitationsService InvitationsService { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; }

    private HubConnection HomeHubConnection;
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    
    private bool IsLoading;
    private bool IsOpen;

    private List<InvitationModel> Invitations = new();

    protected override async Task OnInitializedAsync()
    {
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
        
        IsLoading = true;
        
        await LoadInvitations();
        
        HomeHubConnection.On<InvitationDto>("AddInvitation" ,AddInvitation);
        HomeHubConnection.On<int>("DeleteInvitation" ,DeleteInvitation);

        IsLoading = false;
    }

    private void Toggle()
    {
        IsOpen = !IsOpen;

        if (IsOpen)
        {
            PopoverRef?.Show();
        }
        else
        {
            PopoverRef?.Hide();
        }
    }

    private async Task LoadInvitations()
    {
        var result = await InvitationsService.GetByUserAsync();

        if (result.IsSuccess)
        {
            Invitations = result.Value;
        }
    }
    
    private void AddInvitation(InvitationDto invitation)
    {
        var model = Mapper.Map<InvitationModel>(invitation);
        
        Invitations.Add(model);
        
        StateHasChanged();
        PopoverRef?.Refresh();
    }
    
    private void DeleteInvitation(int invitationId)
    {
        Invitations.RemoveAll(i => i.Id == invitationId);
        
        StateHasChanged();
        PopoverRef?.Refresh();
    }


    private async Task Accept(int invitationId)
    {
        var result =  await InvitationsService.AcceptInvitationAsync(invitationId);
        
        if (result.IsSuccess)
        {
            var acceptedInvitation = Invitations.FirstOrDefault(i => i.Id == invitationId);
            if (acceptedInvitation is not null)
            {
                Invitations.RemoveAll(i => i.Id == invitationId);
                Navigation.NavigateTo($"/board/{acceptedInvitation.Board.Id}");
            }
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
