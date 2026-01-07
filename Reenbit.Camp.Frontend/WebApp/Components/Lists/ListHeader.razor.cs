using Domain.Models.Boards;
using Domain.Models.Lists;
using Domain.Requests.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Services.Abstractions.Services;
using Telerik.Blazor.Components;

namespace WebApp.Components.Lists;

public partial class ListHeader : ComponentBase
{
    [Parameter, EditorRequired]
    public int ListId { get; set; }
    
    [Parameter, EditorRequired]
    public string ListTitle { get; set; }
    
    [Parameter, EditorRequired]
    public EventCallback<MoveListModel> OnMoveList { get; set; }
    
    [CascadingParameter(Name="Board")]
    public BoardInfoModel BoardInfo { get; set; } = default!;
    
    [Inject] 
    public IListsService ListsService { get; set; } = default!;
    
    [Inject]
    public IJSRuntime JsRuntime { get; set; } = default!;
    
    private bool IsEditingTitle = false;
    private string TitleInput = string.Empty;
    
    private TelerikPopup? PopupRef { get; set; }
    private DotNetObjectReference<ListHeader> DotNetRef;
    
    private bool IsMenuOpen = false;
    private bool IsMoveMode = false;
    private int SelectedPosition;

    private List<int> Positions = new List<int>();

    protected override void OnInitialized()
    { 
        DotNetRef = DotNetObjectReference.Create(this);
        
        SelectedPosition = BoardInfo.Lists
            .First(l => l.Id == ListId).Position;
        
        Positions = Enumerable.Range(1, BoardInfo.Lists.Count)
            .ToList();
    }

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
        
    private async Task ToggleMenu()
    {
        IsMenuOpen = !IsMenuOpen;
        
        if (IsMenuOpen)
        {
            PopupRef?.Show();
            await JsRuntime.InvokeVoidAsync("attachClosePopup", DotNetRef, ListId);
        }
        else
        {
            PopupRef?.Hide();
        }
    }

    [JSInvokable("HideMenu")]
    public void HideMenu()
    {
        PopupRef.Hide();
        IsMenuOpen = false;
    }

    private void StartMove()
    {
        HideMenu();
        IsMoveMode = true;
    }

    private async Task SubmitMove()
    {
        await OnMoveList.InvokeAsync(
            new MoveListModel(ListId, SelectedPosition)
        );
        
        IsMoveMode = false;
    }

    private void CancelMove()
    {
        IsMoveMode = false;
    }
    
    public void Dispose()
    {
        DotNetRef?.Dispose();
    }
}