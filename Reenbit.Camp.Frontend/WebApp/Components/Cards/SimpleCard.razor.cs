using Domain.Models.Cards;
using Domain.Requests.Cards;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;
using Telerik.Blazor;

namespace WebApp.Components.Cards;

public partial class SimpleCard : ComponentBase
{
    [Parameter, EditorRequired]
    public CardModel Card { get; set; }

    [Inject] private ICardsService CardsService { get; set; } = default!;
    
    private CardInfo? CardInfoRef;
    
    private bool IsCardOpen = false;
    
    private async Task OnChangeStatus()
    {
        await CardsService.UpdateStatusAsync(
            Card.Id, 
            new UpdateCardStatusRequest(
                Card.Id, 
                Card.IsCompleted));
    }

    private void OpenCard()
    {
        IsCardOpen = true;
    }
    private void CloseCard()
    {
        CardInfoRef?.UpdateCard();
        IsCardOpen = false;
    }
    
    private async Task HandleCradWindowClose(UpdateCardModel updateCardModel)
    {
        Card.Title = updateCardModel.Title;
        Card.IsCompleted = updateCardModel.IsCompleted;
        Card.StartDate = updateCardModel.StartDate;
        Card.DueDate = updateCardModel.DueDate;

        await InvokeAsync(StateHasChanged);
    }

    protected string DateBadgeClass =>
        (Card.DueDate < DateTime.UtcNow && !Card.IsCompleted)
            ? "bg-danger text-white"
            : "bg-success text-white";
}