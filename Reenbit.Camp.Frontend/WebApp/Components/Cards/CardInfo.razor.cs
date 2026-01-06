using Domain.Models.Boards;
using Domain.Models.CardMembers;
using Domain.Models.Cards;
using Domain.Models.Labels;
using Domain.Requests.Cards;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services.Abstractions.Services;


namespace WebApp.Components.Cards;

public partial class CardInfo : ComponentBase
{   
    [Parameter, EditorRequired]
    public int CardId { get; set; }
    
    [Inject]
    private ICardsService CardsService { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public EventCallback<UpdateCardModel> OnCloseWindow { get; set; }
    
    private bool isLoading;
    
    private CardInfoModel Card = new();
    private List<CardLabelModel> CardLabels = new();
    private List<CardMemberModel> CardMembers = new();
    
    private string? InputTitle;
    
    private string? InputDescription;
    
    private bool IsEditingTitle;
    
    private bool IsEditingDescription;
    
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
    
    private void StartEditTitle()
    {
        InputTitle = Card.Title;
        IsEditingTitle = true;
    }

    private async Task SaveTitle()
    {
        if (!string.IsNullOrWhiteSpace(InputTitle))
        {
            Card.Title = InputTitle;

            await CardsService.UpdateAsync(
                CardId, 
                new UpdateCardRequest(
                    Card.Id, 
                    Card.Title,
                    Card.Description));
        }
        
        IsEditingTitle = false;
    }

    private void CancelEdit()
    {
        IsEditingTitle = false;
    }

    private async Task OnTitleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SaveTitle();
        }
        else if (e.Key == "Escape")
        {
            CancelEdit();
        }
    }
    
    private async Task OnChangeStatus()
    {
        await CardsService.UpdateStatusAsync(
            Card.Id, 
            new UpdateCardStatusRequest(
                Card.Id, 
                Card.IsCompleted));
    }
    
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

    private async Task ClearStartDate()
    {
        Card.StartDate = null;
        await UpdateDates();
    }
    
    private async Task ClearDueDate()
    {
        Card.DueDate = null;
        await UpdateDates();
    }
    
    private void OnStartDateStateChange(bool value)
    {
        IsEditingStartDate = value;
    }
    
    private void OnDueDateStateChange(bool value)
    {
        IsEditingDueDate = value;
    }

    private async Task OnStartDateChanged(object value)
    {
        var date = (DateTime?)value;
        if (date == DateTime.MinValue)
        {
            date = null;
        }
        
        Card.StartDate = date;
        IsEditingDueDate = false;
        
        await UpdateDates();
    }
    
    private async Task OnDueDateChanged(object value)
    {
        var date = (DateTime?)value;
        if (date == DateTime.MinValue)
        {
            date = null;
        }
        
        Card.DueDate = date;
        IsEditingDueDate = false;
        
        await UpdateDates();
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

    private async Task UpdateDates()
    {
        await CardsService.UpdateDeadlineAsync(
            CardId,
            new UpdateCardDeadlineRequest(
                Card.Id,
                Card.StartDate,
                Card.DueDate));
    }

    public void UpdateCard()
    {
        OnCloseWindow.InvokeAsync(
            new UpdateCardModel(
                Card.Title, 
                Card.IsCompleted, 
                Card.StartDate, 
                Card.DueDate,
                CardLabels,
                CardMembers));
    }

    private void UpdateCardLabels(List<CardLabelModel> labels)
    {
        CardLabels = labels;
    }
    
    private void UpdateCardMembers(List<CardMemberModel> members)
    {
        CardMembers = members;
    }
}