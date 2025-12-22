using Domain.Models.Cards;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;

namespace WebApp.Components.Cards;

public partial class SimpleCard : ComponentBase
{
    [Parameter, EditorRequired]
    public CardModel Card { get; set; }
    
    protected string DateBadgeClass =>
        (Card.DueDate < DateTime.UtcNow && !Card.IsCompleted)
            ? "bg-danger text-white"
            : "bg-success text-white";
}