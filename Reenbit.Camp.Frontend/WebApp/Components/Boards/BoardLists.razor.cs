using Domain.Models.Cards;
using Domain.Models.Lists;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace WebApp.Components.Boards;

public partial class BoardLists : ComponentBase
{
    [Parameter, EditorRequired]
    public List<ListModel> Lists { get; set; } = new();
    
    private Dictionary<int, TelerikListBox<CardModel>> ListBoxRefs { get; set; } = new();
    
    private Dictionary<int, IEnumerable<CardModel>> ListBoxSelectedItems { get; set; } = new();

    private List<string> ListBoxDropSources = new List<string>();

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

    protected override void OnParametersSet()
    {
        ListBoxDropSources = Lists.Select(l => l.Id.ToString()).ToList();
        
        foreach (var list in Lists)
        {
            if (!ListBoxRefs.ContainsKey(list.Id))
            {
                ListBoxRefs.Add(list.Id, new TelerikListBox<CardModel>());
            }
            
            if (!ListBoxSelectedItems.ContainsKey(list.Id))
            {
                ListBoxSelectedItems.Add(list.Id, new List<CardModel>());
            }
        }
    }
}