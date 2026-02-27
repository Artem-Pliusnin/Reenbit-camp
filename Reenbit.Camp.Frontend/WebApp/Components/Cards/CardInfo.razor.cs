using System.Text.Json;
using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.DTOs.Cards;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models.BoardMembers;
using Domain.Models.Boards;
using Domain.Models.CardMembers;
using Domain.Models.Cards;
using Domain.Models.Labels;
using Domain.Requests.Cards;
using Domain.Responses.CardMembers;
using Domain.Responses.Cards;
using Domain.Responses.Labels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;


namespace WebApp.Components.Cards;

public partial class CardInfo : ComponentBase, IDisposable
{   
    [CascadingParameter(Name="Board")]
    public BoardInfoModel Board { get; set; } = default!;
    
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public int CardId { get; set; }
    
    [Parameter, EditorRequired]
    public EventCallback<UpdateCardModel> OnCloseWindow { get; set; }
    
    [Inject]
    private ICardsService CardsService { get; set; } = default!;
    
    [Inject]
    private IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    private HubConnection TaskHubConnection;

    private List<IDisposable> Subscriptions = new();
    
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
    
    private bool CheckCardUpdatingPermision() => CurrentUser.Role.HasAtLeast(BoardRole.Member);
    
    protected override async Task OnInitializedAsync()
    {
        isLoading = true;

        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
        TaskHubConnection = HubConnectionManager.Get(HubType.TaskHub);
        
        var result = await CardsService.GetInfoAsync(Board.Id, CardId);

        if (result.IsSuccess)
        {
            Card = result.Value;
            isLoading = false;
        }
        
        Subscriptions.Add(TaskHubConnection
            .On<UpdatedCardTitleDto>(
                SubscribeTaskHubConstants.UpdateCardTitle, 
                UpdateCardTitle));
        
        Subscriptions.Add(TaskHubConnection
            .On<UpdatedCardDescriptionDto>(
                SubscribeTaskHubConstants.UpdateCardDescription, 
                UpdateCardDescription));
        
        Subscriptions.Add(TaskHubConnection
            .On<UpdatedCardDatesDto>(
                SubscribeTaskHubConstants.UpdateCardDates, 
                UpdateCardDates));
        
        Subscriptions.Add(TaskHubConnection
            .On<UpdatedCardStatusDto>(
                SubscribeTaskHubConstants.UpdateCardStatus, 
                UpdateCardStatus));
    }
    
    private void StartEditTitle()
    {
        if (CheckCardUpdatingPermision())
        {
            InputTitle = Card.Title;
            IsEditingTitle = true;
        }
    }

    private async Task SaveTitle()
    {
        if (!string.IsNullOrWhiteSpace(InputTitle))
        {
            Card.Title = InputTitle;

            await CardsService.UpdateAsync(
                Board.Id, 
                CardId, 
                new UpdateCardRequest(
                    Card.Id, 
                    Card.Title,
                    Card.Description));

            var dto = new UpdatedCardTitleDto(CardId, Card.Title);
            
            await TaskHubConnection
                .SendAsync(
                    SendTaskHubConstants.UpdateCardTitle, 
                    dto, 
                    Card.Id);
                
            await HomeHubConnection
                .SendAsync(
                    SendHomeHubConstants.UpdateCardTitle, 
                    dto, 
                    Board.Id);
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
            Board.Id, 
            Card.Id, 
            new UpdateCardStatusRequest(
                Card.Id, 
                Card.IsCompleted));

        var dto = new UpdatedCardStatusDto(Card.Id, Card.IsCompleted);
        
        await HomeHubConnection
            .SendAsync(
                SendHomeHubConstants.UpdateCardStatus, 
                dto, 
                Board.Id);
        
        await TaskHubConnection
            .SendAsync(
                SendTaskHubConstants.UpdateCardStatus, 
                dto, 
                Card.Id);
    }
    
    private void StartEditDescription()
    {
        if (CheckCardUpdatingPermision())
        {
            InputDescription = Card.Description;
            IsEditingDescription = true;
        }
    }

    private async Task SaveDescription()
    {
        Card.Description = InputDescription;
        
        await CardsService.UpdateAsync(
            Board.Id,
            CardId, 
            new UpdateCardRequest(
                Card.Id, 
                Card.Title,
                Card.Description));
        
        await TaskHubConnection
            .SendAsync(
                SendTaskHubConstants.UpdateCardDescription, 
                new UpdatedCardDescriptionDto(Card.Id, Card.Description), 
                Card.Id);

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
        if (CheckCardUpdatingPermision())
        {
            IsEditingStartDate = value;
        }
    }
    
    private void OnDueDateStateChange(bool value)
    {
        if (CheckCardUpdatingPermision())
        {
            IsEditingDueDate = value;
        }
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

    private async Task UpdateDates()
    {
        await CardsService.UpdateDeadlineAsync(
            Board.Id,
            CardId,
            new UpdateCardDeadlineRequest(
                Card.Id,
                Card.StartDate,
                Card.DueDate));

        var dto = new UpdatedCardDatesDto(CardId, Card.StartDate, Card.DueDate);
        
        await HomeHubConnection
            .SendAsync(
                SendHomeHubConstants.UpdateCardDates, 
                dto, 
                Board.Id);
        
        await TaskHubConnection
            .SendAsync(
                SendTaskHubConstants.UpdateCardDates, 
                dto, 
                Card.Id);
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

    private async Task UpdateCardLabels(List<CardLabelModel> labels)
    {
        CardLabels = labels;
        
        var cardLabelDtos = Mapper.Map<List<CardLabelDto>>(labels);

        await HomeHubConnection
            .SendAsync(
                SendHomeHubConstants.UpdateCardLabels, 
                new UpdatedCardLabelsDto(
                    CardId, 
                    cardLabelDtos), 
                Board.Id);
    }
    
    private async Task UpdateCardMembers(List<CardMemberModel> members)
    {
        CardMembers = members;
        
        var cardMembersDtos = Mapper.Map<List<CardMemberDto>>(members);

        await HomeHubConnection
            .SendAsync(
                SendHomeHubConstants.UpdateCardMembers, 
                new UpdatedCardMembersDto(
                    CardId, 
                    cardMembersDtos), 
                Board.Id);
    }

    private void UpdateCardTitle(UpdatedCardTitleDto dto)
    {
        if (Card.Id == dto.CardId)
        {
            Card.Title = dto.Title;
        }
        StateHasChanged();
    }

    private void UpdateCardDescription(UpdatedCardDescriptionDto dto)
    {
        if (Card.Id == dto.CardId)
        {
            Card.Description = dto.Description;
        }
        StateHasChanged();
    }
    
    private void UpdateCardDates(UpdatedCardDatesDto dto)
    {
        if (Card.Id == dto.CardId)
        {
            Card.StartDate = dto.StartDate;
            Card.DueDate = dto.DueDate;
        }
        StateHasChanged();
    }
    
    private void UpdateCardStatus(UpdatedCardStatusDto dto)
    {
        if (Card.Id == dto.CardId)
        {
            Card.IsCompleted = dto.IsCompleted;
        }
        StateHasChanged();
    }
    

    public void Dispose()
    {
        foreach (var subscription in Subscriptions)
        {
            subscription.Dispose();
        }
    }
}