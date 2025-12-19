using Domain.Models.Cards;
using Domain.Models.Lists;
using Domain.Requests.Cards;
using Domain.Requests.Lists;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;
using Telerik.Blazor.Components;

namespace WebApp.Components.Boards;

public partial class BoardLists : ComponentBase
{
    [Parameter, EditorRequired]
    public int BoardId { get; set; }
    
    [Parameter, EditorRequired]
    public List<ListModel> Lists { get; set; } = new();
    
    [Inject] 
    public IListsService ListsService { get; set; } = default!;
    
    [Inject] 
    public ICardsService CardsService { get; set; } = default!;
    
    private Dictionary<int, TelerikListBox<CardModel>> ListBoxRefs { get; set; } = new();
    
    private Dictionary<int, IEnumerable<CardModel>> ListBoxSelectedItems { get; set; } = new();

    private List<string> ListBoxDropSources =>
        Lists.Select(l => l.Id.ToString()).ToList();
    
    private string NewListTitle = string.Empty;
    
    private Dictionary<int, string> NewCardTitles = new();

    private async Task AddCard(int listId)
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

        var list = Lists.First(l => l.Id == listId);

        list.Cards.Add(result.Value);

        NewCardTitles[listId] = string.Empty;

        ListBoxRefs[listId].Rebind();
    }
    
    private async Task AddList()
    {
        if (string.IsNullOrWhiteSpace(NewListTitle))
        {
            return;
        }
        
        var result = await ListsService
            .CreateAsync(new CreateListRequest(BoardId, NewListTitle));

        if (result.IsFailure || result.Value == null)
        {
            return;
        }
        
        Lists.Add(result.Value);
        
        NewListTitle = string.Empty;
        
        ListBoxRefs.Add(result.Value.Id, new TelerikListBox<CardModel>());
            
        ListBoxSelectedItems.Add(result.Value.Id, new List<CardModel>());

        NewCardTitles.Add(result.Value.Id, "");
        
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
        var list = Lists.First(l => l.Id.ToString() == listBoxId);
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

    protected override void OnInitialized()
    {
        foreach (var list in Lists)
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
    }
}