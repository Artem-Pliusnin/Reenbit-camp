using Domain.Requests.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services.Abstractions.Services;

namespace WebApp.Components.Lists;

public partial class ListHeader : ComponentBase
{
    [Parameter, EditorRequired]
    public int ListId { get; set; }
    
    [Parameter, EditorRequired]
    public string ListTitle { get; set; }
    
    [Inject] 
    public IListsService ListsService { get; set; } = default!;
    
    private bool IsEditingTitle = false;
    
    private string TitleInput = string.Empty;

    private void StartEditTitle()
    {
        TitleInput = ListTitle;
        IsEditingTitle = true;
    }

    private async Task SaveTitle()
    {
        if (!string.IsNullOrWhiteSpace(TitleInput))
        {
            ListTitle = TitleInput;

            await ListsService.UpdateAsync(
                ListId, 
                new UpdateListRequest(ListId, TitleInput));
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
}