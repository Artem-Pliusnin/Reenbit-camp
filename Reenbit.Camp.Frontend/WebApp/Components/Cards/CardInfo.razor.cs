using System.ComponentModel.DataAnnotations;
using Domain.Models.Cards;
using Domain.Requests.Cards;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;
using Telerik.Blazor.Components;

namespace WebApp.Components.Cards;

public partial class CardInfo : ComponentBase
{
    [Parameter, EditorRequired]
    public int CardId { get; set; }
    
    [Inject]
    private ICardsService CardsService { get; set; } = default!;
    
    private bool isLoading;
    
    private CardInfoModel Card = new();
    
    private bool IsEditingDescription;
    
    private string? InputDescription;
    
    private bool IsEditingStartDate;
    
    private bool IsEditingDueDate;
    
    private DateTime DueDateMin => Card.StartDate == null 
        ? new DateTime(2000, 1, 1, 0, 0, 0)
        : Card.StartDate.Value;
    
    private DateTime DueDateMax => DateTime.Now.AddYears(20);
    
    private DateTime StartDateMin => new DateTime(2000, 1, 1, 0, 0, 0);
    
    private DateTime StartDateMax => Card.DueDate == null 
        ? DateTime.Now.AddYears(10)
        : Card.DueDate.Value;
    
    private string DateBadgeClass =>
        Card.DueDate == null ? ""
        : (Card.DueDate < DateTime.UtcNow && !Card.IsCompleted)
            ? "bg-danger text-white"
            : "bg-success text-white";
    
    private void StartEditDescription()
    {
        InputDescription = Card.Description;
        IsEditingDescription = true;
    }

    private async Task SaveDescription()
    {
        Card.Description = InputDescription;
        
        await CardsService.UpdateAsync(
            CardId, 
            new UpdateCardRequest(
                Card.Id, 
                Card.Title,
                Card.Description));

        IsEditingDescription = false;
    }

    private void CancelEditDescription()
    {
        InputDescription = null;
        IsEditingDescription = false;
    }

    private void ClearStartDate()
    {
        Card.StartDate = null;
    }
    
    private void ClearDueDate()
    {
        Card.DueDate = null;
    }
    
    private void OnStartDateOpen()
    {
        IsEditingStartDate = true;
    }
    
    private void OnStartDateClose()
    {
        IsEditingStartDate = false;
    }

    private void OnDueDateOpen()
    {
        IsEditingDueDate = true;
    }
    
    private void OnDueDateClose()
    {
        IsEditingDueDate = false;
    }

    
    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        var result = await CardsService.GetInfoAsync(CardId);

        if (result.IsSuccess)
        {
            Card = result.Value;
            isLoading = false;
        }
    }
}