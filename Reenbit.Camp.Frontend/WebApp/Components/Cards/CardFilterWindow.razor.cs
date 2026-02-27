using System.Timers;
using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.Models.CardMembers;
using Domain.Models.Cards;
using Domain.Models.Labels;
using Domain.Responses.Cards;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Services.Abstractions.Services;
using Services.HubServices;

namespace WebApp.Components.Cards;

public partial class CardFilterWindow : ComponentBase
{
    [Parameter, EditorRequired]
    public int BoardId { get; set; }
    
    [Parameter] 
    public EventCallback<CardModel> OnCardUpdated { get; set; }
    
    [Parameter] 
    public EventCallback<int> OnCardDeleted { get; set; }
    
    [Inject] 
    public ICardsService CardsService { get; set; } = default!;
    
    [Inject] 
    public ILabelsService LabelsService { get; set; } = default!;
    
    [Inject] 
    public IJSRuntime JsRuntime { get; set; } = default!;
    
    [Inject] 
    public IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    private List<IDisposable> Subscriptions = new();
    
    private List<LabelModel> AllLabels = new();

    private CardsFilterModel filter = new()
    {
        PageSize = 8,
        Page = 1,
    };

    private List<int>? SelectedLabels = new();

    private List<CardModel> Cards = new();

    private bool isLoading;
    
    private bool hasMore = true;

    private ElementReference _scrollRef;

    private System.Timers.Timer aTimer = default!;

    protected override async Task OnInitializedAsync()
    {
        filter.BoardId = BoardId;
        
        var labelsResult = await LabelsService.GetByBoardAsync(BoardId);
        
        if (labelsResult.IsSuccess)
        {
            AllLabels = labelsResult.Value;
        }
        
        await LoadCardsAsync();

        aTimer = new System.Timers.Timer(400);
        aTimer.Elapsed += OnTimerElapsed;
        aTimer.AutoReset = false;
        
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
        
        Subscriptions.Add(HomeHubConnection
            .On<int>(SubscribeHomeHubConstants.RemoveCard, HandelRemovingCard));
        
        Subscriptions.Add(HomeHubConnection
            .On<UpdatedCardTitleDto>(SubscribeHomeHubConstants.UpdateCardTitle, UpdateCardTitle));
        
        Subscriptions.Add(HomeHubConnection
            .On<UpdatedCardDatesDto>(SubscribeHomeHubConstants.UpdateCardDates, UpdateCardDates));
        
        Subscriptions.Add(HomeHubConnection
            .On<UpdatedCardStatusDto>(SubscribeHomeHubConstants.UpdateCardStatus, UpdateCardStatus));
        
        Subscriptions.Add(HomeHubConnection
            .On<UpdatedCardLabelsDto>(SubscribeHomeHubConstants.UpdateCardLabels, UpdateCardLabels));
        
        Subscriptions.Add(HomeHubConnection
            .On<UpdatedCardMembersDto>(SubscribeHomeHubConstants.UpdateCardMembers, UpdateCardMembers));
    }
    
    private void ResetTimer(KeyboardEventArgs e)
    {
        aTimer.Stop();
        aTimer.Start();
    }

    private async void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        await ResetAndReload();
    }

    private async void OnOnlyMineChanged()
    {
        await ResetAndReload();
    }
    
    private async void OnLabelsChanged()
    {
        await ResetAndReload();
    }
    
    private async Task ResetAndReload()
    {
        Cards.Clear();
        filter.Page = 1;
        hasMore = true;

        await LoadCardsAsync();
    }
    
    private async Task LoadCardsAsync()
    {
        if (isLoading || !hasMore)
        {
            return;
        }

        isLoading = true;

        if (SelectedLabels?.Count == 0)
        {
            SelectedLabels = null;
        }

        var result = await CardsService.GetFilteredCardsAsync(
            BoardId, 
            filter, 
            SelectedLabels);

        if (result.IsSuccess)
        {
            Cards.AddRange(result.Value.Dtos);
            
            hasMore = result.Value.HasMore;
            
            filter.Page++;
        }

        isLoading = false;
        StateHasChanged();
    }

    private async Task HandleScroll()
    {
        var info = await JsRuntime.InvokeAsync<ScrollInfo>(
            "getScrollInfo",
            _scrollRef);

        if (info.IsNearBottom)
        {
            await LoadCardsAsync();
        }
    }

    private async Task RemoveCard(CardModel card)
    {
        Cards.RemoveAll(c => c.Id == card.Id);
        
        await OnCardDeleted.InvokeAsync(card.Id);

        StateHasChanged();
    }

    public void Dispose()
    {
        aTimer?.Dispose();
    }
    private class ScrollInfo
    {
        public bool IsNearBottom { get; set; }
    }
    
    public void HandelRemovingCard(int cardId)
    {
        Cards.RemoveAll(c => c.Id == cardId);

        StateHasChanged();
    }
    
    private void UpdateCardTitle(UpdatedCardTitleDto dto)
    {
        var card = Cards.FirstOrDefault(c => c.Id == dto.CardId);

        if (card is null)
        {
            return;
        }
        
        card.Title = dto.Title;
        
        StateHasChanged();
    }
    
    private void UpdateCardDates(UpdatedCardDatesDto dto)
    {
        var card = Cards.FirstOrDefault(c => c.Id == dto.CardId);

        if (card is null)
        {
            return;
        }
        
        card.StartDate = dto.StartDate;
        card.DueDate = dto.DueDate;
        
        StateHasChanged();
    }
    
    private void UpdateCardStatus(UpdatedCardStatusDto dto)
    {
        var card = Cards.FirstOrDefault(c => c.Id == dto.CardId);

        if (card is null)
        {
            return;
        }
        
        card.IsCompleted = dto.IsCompleted;
        
        StateHasChanged();
    }
    
    private void UpdateCardLabels(UpdatedCardLabelsDto dto)
    {
        var card = Cards.FirstOrDefault(c => c.Id == dto.CardId);

        if (card is null)
        {
            return;
        }
        
        var cardLabelModels = Mapper.Map<List<CardLabelModel>>(dto.Labels);

        card.Labels = cardLabelModels;
        
        StateHasChanged();
    }
    
    private void UpdateCardMembers(UpdatedCardMembersDto dto)
    {
        var card = Cards.FirstOrDefault(c => c.Id == dto.CardId);

        if (card is null)
        {
            return;
        }
        
        var cardMemberModels = Mapper.Map<List<CardMemberModel>>(dto.Members);

        card.Members = cardMemberModels;
        
        StateHasChanged();
    }
    
    private async Task UpdateCard(CardModel card)
    {
        await OnCardUpdated.InvokeAsync(card);
    }
    
    public void RefreshCard(CardModel updatedCard)
    {
        var card = Cards.FirstOrDefault(c => c.Id == updatedCard.Id);
        
        if (card is null)
        {
            return;
        }
        
        card.Title = updatedCard.Title;
        card.IsCompleted = updatedCard.IsCompleted;
        card.StartDate = updatedCard.StartDate;
        card.DueDate = updatedCard.DueDate;
        card.Labels = updatedCard.Labels;
        card.Members = updatedCard.Members;
        
        StateHasChanged();
    }
}