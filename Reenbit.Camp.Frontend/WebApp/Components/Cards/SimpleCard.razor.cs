using Domain.Models.BoardMembers;
using Domain.Models.Boards;
using Domain.Models.Cards;
using Domain.Requests.Cards;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;
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
}