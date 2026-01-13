using Domain.Models.BoardMembers;
using Domain.Models.Boards;
using Domain.Models.Cards;
using Domain.Requests.Cards;
using Domain.Responses.Cards;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor;

namespace WebApp.Components.Cards;

public partial class SimpleCard : ComponentBase
{
    [CascadingParameter(Name="Board")]
    public BoardInfoModel Board { get; set; } = default!;
    
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;

    [Parameter, EditorRequired]
    public CardModel Card { get; set; }
    
    [Parameter, EditorRequired]
    public EventCallback<CardModel> OnDeleteCard { get; set; }

    [Inject] 
    private ICardsService CardsService { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    
    private CardInfo? CardInfoRef;
    
    private bool IsCardOpen = false;
    
    private bool IsDeleteConfirmOpen = false;

    private async Task OnChangeStatus()
    {
        await CardsService.UpdateStatusAsync(
            Card.Id, 
            new UpdateCardStatusRequest(
                Card.Id, 
                Card.IsCompleted));
        
        await HomeHubConnection
            .SendAsync(
                "UpdateCardStatus", 
                new UpdatedCardStatusDto(
                    Card.Id,
                    Card.IsCompleted), 
                Board.Id);
    }

    private void OpenCard()
    {
        IsCardOpen = true;
    }
    private void CloseCard()
    {
        CardInfoRef?.UpdateCard();
        IsDeleteConfirmOpen = false;
        IsCardOpen = false;
    }
    
    private void OpenDeleteConfirm()
    {
        IsDeleteConfirmOpen = true;
        
        StateHasChanged();
    }

    private void CancelDelete()
    {
        IsDeleteConfirmOpen = false;
    }
    
    private async Task ConfirmDelete()
    {
        IsDeleteConfirmOpen = false;
        
        IsCardOpen = false;
        
        var result = await CardsService.DeleteAsync(Card.Id);

        if (result.IsSuccess)
        {
            await OnDeleteCard.InvokeAsync(Card);
            await HomeHubConnection
                .SendAsync("DeleteCard", Card.Id, Board.Id);
        }
    }
    
    private async Task HandleCardWindowClose(UpdateCardModel updateCardModel)
    {
        Card.Title = updateCardModel.Title;
        Card.IsCompleted = updateCardModel.IsCompleted;
        Card.StartDate = updateCardModel.StartDate;
        Card.DueDate = updateCardModel.DueDate;
        Card.Labels = updateCardModel.Labels;
        Card.Members = updateCardModel.Members;

        await InvokeAsync(StateHasChanged);
    }

    protected string DateBadgeClass =>
        (Card.DueDate < DateTime.UtcNow && !Card.IsCompleted)
            ? "bg-danger text-white"
            : "bg-success text-white";
    
    protected override void OnInitialized()
    {
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
    }
}