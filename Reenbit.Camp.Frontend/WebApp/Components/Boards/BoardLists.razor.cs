using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models.BoardMembers;
using Domain.Models.Boards;
using Domain.Models.CardMembers;
using Domain.Models.Cards;
using Domain.Models.Labels;
using Domain.Models.Lists;
using Domain.Requests.Cards;
using Domain.Requests.Lists;
using Domain.Responses.Cards;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;

namespace WebApp.Components.Boards;

public partial class BoardLists : ComponentBase, IDisposable
{
    [CascadingParameter(Name="Board")]
    public BoardInfoModel Board { get; set; } = default!;
    
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;
    
    [Inject] 
    public IListsService ListsService { get; set; } = default!;
    
    [Inject] 
    public ICardsService CardsService { get; set; } = default!;
    
    [Inject]
    public IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    private List<IDisposable> Subscriptions = new();
    
    private Dictionary<int, TelerikListBox<CardModel>> ListBoxRefs { get; set; } = new();
    
    private Dictionary<int, IEnumerable<CardModel>> ListBoxSelectedItems { get; set; } = new();

    private List<string> ListBoxDropSources =>
        Board.Lists.Select(l => l.Id.ToString()).ToList();
    
    private string NewListTitle = string.Empty;
    
    private Dictionary<int, string> NewCardTitles = new();

    private bool CheckListCreatingPermision() => CurrentUser.Role.HasAtLeast(BoardRole.Member);
    
    private bool CheckCardCreatingPermision() => CurrentUser.Role.HasAtLeast(BoardRole.Member);
    
    private bool CheckCardMovingPermision() => CurrentUser.Role.HasAtLeast(BoardRole.Member);

    private async Task CreateCard(int listId)
    {
        var title = NewCardTitles[listId];
        
        if (string.IsNullOrWhiteSpace(title))
        {
            return;
        }
        
        var result = await CardsService
            .CreateAsync(new CreateCardRequest(listId, title));

        if (result.IsFailure || result.Value == null)
        {
            return;
        }

        var list = Board.Lists.First(l => l.Id == listId);

        list.Cards.Add(result.Value);

        NewCardTitles[listId] = string.Empty;

        ListBoxRefs[listId].Rebind();
        
        await HomeHubConnection
            .SendAsync(SendHomeHubConstants.AddCard, result.Value, list.Id, Board.Id);
    }
    
    private async Task AddList()
    {
        if (string.IsNullOrWhiteSpace(NewListTitle))
        {
            return;
        }
        
        var result = await ListsService
            .CreateAsync(new CreateListRequest(Board.Id, NewListTitle));

        if (result.IsSuccess)
        {
            await AddListToBoard(result.Value);
            
            await HomeHubConnection
                .SendAsync(SendHomeHubConstants.AddList , result.Value, Board.Id);
        }
    }

    private async Task AddListToBoard(ListModel listModel)
    {
        Board.Lists.Add(listModel);
        
        NewListTitle = string.Empty;
        
        ListBoxRefs.Add(listModel.Id, new TelerikListBox<CardModel>());
            
        ListBoxSelectedItems.Add(listModel.Id, new List<CardModel>());

        NewCardTitles.Add(listModel.Id, "");
        
        foreach (var listBoxRef in ListBoxRefs)
        {
            listBoxRef.Value.Rebind();
        }
        
        await InvokeAsync(StateHasChanged);
    }

    private async void OnListBoxDrop(
        ListBoxDropEventArgs<CardModel> args,
        string sourceListBoxId,
        List<CardModel> sourceData)
    {
        var destinationIndex = args.DestinationIndex ?? 0;
        var destinationData = GetListBoxDataFromId(args.DestinationListBoxId);

        if (args.DestinationListBoxId == sourceListBoxId)
        {
            ReorderItems(args.Items, sourceData, destinationIndex);
        }
        else
        {
            MoveItems(args.Items, sourceData, destinationData, destinationIndex);
        }

        foreach (var listBoxRef in ListBoxRefs)
        {
            listBoxRef.Value.Rebind();
        }

        await PersistCardMove(
            args.Items,
            int.Parse(args.DestinationListBoxId),
            destinationIndex
        );
        
        var reorderEvent = new CardsReorderedModel()
        {
            BoardId = Board.Id,
            SourceListId = int.Parse(sourceListBoxId),
            DestinationListId = int.Parse(args.DestinationListBoxId),
            OrderedCardIds = destinationData.Select(c => c.Id).ToList()
        };
        
        await HomeHubConnection.SendAsync(
            SendHomeHubConstants.CardsReordered,
            reorderEvent
        );
    }

    private void ReorderItems(
        List<CardModel> items,
        List<CardModel> collection,
        int destinationIndex)
    {
        collection.RemoveAll(x => items.Contains(x));

        if (destinationIndex >= 0)
        {
            collection.InsertRange(destinationIndex, items);
        }
        else
        {
            collection.AddRange(items);
        }
    }

    private void MoveItems(
        List<CardModel> items,
        List<CardModel> sourceData,
        List<CardModel> destinationData,
        int destinationIndex)
    {
        foreach (var item in items)
        {
            sourceData.RemoveAll(x => items.Any(y => y.Id == x.Id));

            if (destinationIndex >= 0)
            {
                destinationData.Insert(destinationIndex, item);
            }
            else
            {
                destinationData.Add(item);
            }
        }
    }

    private List<CardModel> GetListBoxDataFromId(string listBoxId)
    {
        var list = Board.Lists.First(l => l.Id.ToString() == listBoxId);
        return list.Cards;
    }
    
    private async Task PersistCardMove(
        List<CardModel> cards,
        int newListId,
        int startPosition)
    {
        var requests = cards.Select((card, index) =>
            new UpdateCardPositionRequest(
                card.Id,
                newListId,
                startPosition + index + 1
            )
        ).ToList();
        
        foreach (var request in requests)
        { 
            await CardsService.UpdatePositionAsync(request.CardId, request);
        }
    }
    
    private async Task HandleMoveList(MoveListModel moveListModel)
    {
        await MoveListOnBoard(moveListModel);
        
        await ListsService.UpdatePositionAsync(
            moveListModel.ListId,
            new UpdateListPositionRequest(
                moveListModel.ListId,
                moveListModel.NewPosition
            )
        );
        
        await HomeHubConnection
            .SendAsync(SendHomeHubConstants.UpdateListPosition, moveListModel, Board.Id);
    }

    private async Task MoveListOnBoard(MoveListModel moveListModel)
    {
        var list = Board.Lists.First(l => l.Id == moveListModel.ListId);

        Board.Lists.Remove(list);
        Board.Lists.Insert(moveListModel.NewPosition - 1, list);

        for (int i = 0; i < Board.Lists.Count; i++)
        {
            Board.Lists[i].Position = i + 1;
        }
        
        await InvokeAsync(StateHasChanged);
    }
    
    private void RemoveList(ListModel listModel)
    {
        Board.Lists.Remove(listModel);
        ListBoxRefs.Remove(listModel.Id);
        ListBoxSelectedItems.Remove(listModel.Id);
        NewCardTitles.Remove(listModel.Id);
        
        for (int i = 0; i < Board.Lists.Count; i++)
        {
            Board.Lists[i].Position = i + 1;
        }
        
        StateHasChanged();
    }
    
    private void UpdateList(UpdateListModel dto)
    {
        var list = Board.Lists.FirstOrDefault(l => l.Id == dto.Id);
        if (list != null)
        {
            list.Title = dto.Title;
        }
        
        StateHasChanged();
    }

    private void DeleteList(int listId)
    {
        var list = Board.Lists.FirstOrDefault(l => l.Id == listId);
        if (list != null)
        {
            RemoveList(list);
        }
    }
    
    private void ApplyCardsReorder(CardsReorderedModel dto)
    {
        var sourceList = Board.Lists.First(l => l.Id == dto.SourceListId);
        var destinationList = Board.Lists.First(l => l.Id == dto.DestinationListId);
        
        var allCards = sourceList.Cards
            .Concat(destinationList.Cards)
            .GroupBy(c => c.Id)
            .ToDictionary(g => g.Key, g => g.First());
        
        if (sourceList.Id == destinationList.Id)
        {
            sourceList.Cards.Clear();
            sourceList.Cards.AddRange(
                dto.OrderedCardIds
                    .Where(id => allCards.ContainsKey(id))
                    .Select(id => allCards[id])
            );
        }
        else
        {
            sourceList.Cards.RemoveAll(c => 
                dto.OrderedCardIds.Any(id => id == c.Id));
            
            destinationList.Cards.Clear();
            destinationList.Cards.AddRange(
                dto.OrderedCardIds
                    .Where(id => allCards.ContainsKey(id))
                    .Select(id => allCards[id])
            );
        }
        
        ListBoxRefs[dto.SourceListId].Rebind();
        ListBoxRefs[dto.DestinationListId].Rebind();
        StateHasChanged();
    }

    
    private void RemoveCard(CardModel card)
    {
        var list = Board.Lists
            .FirstOrDefault(l => l.Cards.Any(c => c.Id == card.Id));

        if (list == null)
        {
            return;
        }

        list.Cards.Remove(card);
        
        ListBoxRefs[list.Id].Rebind();

        StateHasChanged();
    }
    
    private void HandelRemovingCard(int cardId)
    {
        var list = Board.Lists
            .FirstOrDefault(l => l.Cards.Any(c => c.Id == cardId));

        if (list == null)
        {
            return;
        }

        list.Cards.RemoveAll(c => c.Id == cardId);
        
        ListBoxRefs[list.Id].Rebind();

        StateHasChanged();
    }
    
    private void AddCard(CreatedCardDto dto)
    {
        var list = Board.Lists.First(l => l.Id == dto.ListId);
        
        var card = Mapper.Map<CardModel>(dto.Card);
        
        list.Cards.Add(card);

        ListBoxRefs[list.Id].Rebind();
        
        StateHasChanged();
    }

    private CardModel FindCard(int cardId)
    {
        return Board.Lists
            .SelectMany(l => l.Cards)
            .First(c => c.Id == cardId);
    }

    private void UpdateCardTitle(UpdatedCardTitleDto dto)
    {
        var card = FindCard(dto.CardId);
        
        card.Title = dto.Title;
        
        StateHasChanged();
    }
    
    private void UpdateCardDates(UpdatedCardDatesDto dto)
    {
        var card = FindCard(dto.CardId);
        
        card.StartDate = dto.StartDate;
        card.DueDate = dto.DueDate;
        
        StateHasChanged();
    }
    
    private void UpdateCardStatus(UpdatedCardStatusDto dto)
    {
        var card = FindCard(dto.CardId);
        
        card.IsCompleted = dto.IsCompleted;
        
        StateHasChanged();
    }
    
    private void UpdateCardLabels(UpdatedCardLabelsDto dto)
    {
        var card = FindCard(dto.CardId);
        
        var cardLabelModels = Mapper.Map<List<CardLabelModel>>(dto.Labels);

        card.Labels = cardLabelModels;
        
        StateHasChanged();
    }
    
    private void UpdateCardMembers(UpdatedCardMembersDto dto)
    {
        var card = FindCard(dto.CardId);
        
        var cardMemberModels = Mapper.Map<List<CardMemberModel>>(dto.Members);

        card.Members = cardMemberModels;
        
        StateHasChanged();
    }
    
    protected override async Task OnInitializedAsync()
    {
        foreach (var list in Board.Lists)
        {
            if (!ListBoxRefs.ContainsKey(list.Id))
            {
                ListBoxRefs[list.Id] = new TelerikListBox<CardModel>();
            }

            if (!ListBoxSelectedItems.ContainsKey(list.Id))
            {
                ListBoxSelectedItems[list.Id] = new List<CardModel>();
            }

            if (!NewCardTitles.ContainsKey(list.Id))
            {
                NewCardTitles[list.Id] = string.Empty;
            }
        }
        
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
        
        Subscriptions.Add(HomeHubConnection
            .On<MoveListModel>(SubscribeHomeHubConstants.MoveList, MoveListOnBoard));
        
        Subscriptions.Add(HomeHubConnection
            .On<int>(SubscribeHomeHubConstants.DeleteList, DeleteList));
        
        Subscriptions.Add(HomeHubConnection
            .On<UpdateListModel>(SubscribeHomeHubConstants.UpdateList, UpdateList));
        
        Subscriptions.Add(HomeHubConnection
            .On<ListModel>(SubscribeHomeHubConstants.AddList, AddListToBoard));

        Subscriptions.Add(HomeHubConnection
            .On<CardsReorderedModel>(SubscribeHomeHubConstants.CardsReordered, ApplyCardsReorder));
        
        Subscriptions.Add(HomeHubConnection
            .On<CreatedCardDto>(SubscribeHomeHubConstants.AddCard, AddCard));
        
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
    
    public void Dispose()
    {
        foreach (var subscription in Subscriptions)
        {
            subscription.Dispose();
        }
    }
}