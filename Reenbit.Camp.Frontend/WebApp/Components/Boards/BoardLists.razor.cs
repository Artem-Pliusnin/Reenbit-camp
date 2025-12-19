using Domain.Models.Cards;
using Domain.Models.Lists;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace WebApp.Components.Boards;

public partial class BoardLists : ComponentBase
{
    [Parameter, EditorRequired]
    public int BoardId { get; set; }
    
    [Parameter, EditorRequired]
    public List<ListModel> Lists { get; set; } = new();
    
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

        var list = Lists.First(l => l.Id == listId);

        list.Cards.Add(new CardModel
        {
            Id = Random.Shared.Next(),
            Title = title,
            Position = list.Cards.Count,
            IsCompleted = false
        });

        NewCardTitles[listId] = string.Empty;

        ListBoxRefs[listId].Rebind();
    }
    
    private async Task AddList()
    {
        if (string.IsNullOrWhiteSpace(NewListTitle))
        {
            return;
        }

        var newList = new ListModel
        {
            Id = Random.Shared.Next(),
            Title = NewListTitle,
            Position = Lists.Count,
            Cards = new List<CardModel>()
        };

        Lists.Add(newList);
        
        NewListTitle = string.Empty;
        
        ListBoxRefs.Add(newList.Id, new TelerikListBox<CardModel>());
            
        ListBoxSelectedItems.Add(newList.Id, new List<CardModel>());

        NewCardTitles.Add(newList.Id, "");
        
        foreach (var listBoxRef in ListBoxRefs)
        {
            listBoxRef.Value.Rebind();
        }
        
        await InvokeAsync(StateHasChanged);
    }

    private void OnListBoxDrop(
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