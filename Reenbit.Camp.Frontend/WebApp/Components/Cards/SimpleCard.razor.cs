using Domain.Models.Cards;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;

namespace WebApp.Components.Cards;

public partial class SimpleCard : ComponentBase
{
    [Parameter, EditorRequired]
    public CardModel Card { get; set; }
    
    private bool IsCardOpen = false;

    private void OpenCard()
    {
        IsCardOpen = true;
    }
    private void CloseCard()
    {
        IsCardOpen = false;
    }

    protected string DateBadgeClass =>
        (Card.DueDate < DateTime.UtcNow && !Card.IsCompleted)
            ? "bg-danger text-white"
            : "bg-success text-white";
}