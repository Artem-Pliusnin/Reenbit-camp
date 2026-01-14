using Domain.Constants.HubConstants;
using Domain.Models.Boards;
using Domain.Models.Lists;
using Domain.Requests.Lists;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;

namespace WebApp.Components.Lists;

public partial class ListHeader : ComponentBase
{
    [Parameter, EditorRequired]
    public ListModel List { get; set; }
    
    [Parameter, EditorRequired]
    public EventCallback<MoveListModel> OnMoveList { get; set; }
    
    [Parameter, EditorRequired]
    public EventCallback<ListModel> OnDeleteList { get; set; }
    
    [CascadingParameter(Name="Board")]
    public BoardInfoModel BoardInfo { get; set; } = default!;
    
    [Inject] 
    public IListsService ListsService { get; set; } = default!;
    
    [Inject]
    public IJSRuntime JsRuntime { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection HomeHubConnection;
    
    private bool IsEditingTitle = false;
    private string TitleInput = string.Empty;
    
    private TelerikPopup? PopupRef { get; set; }
    private DotNetObjectReference<ListHeader> DotNetRef;
    
    private bool IsMenuOpen = false;
    private bool IsMoveMode = false;
    private int SelectedPosition;

    private List<int> Positions = new List<int>();
    
    private bool IsDeleteConfirmOpen = false;

    protected override void OnInitialized()
    { 
        DotNetRef = DotNetObjectReference.Create(this);
        HomeHubConnection = HubConnectionManager.Get(HubType.HomeHub);
    }

    private void StartEditTitle()
    {
        TitleInput = List.Title;
        IsEditingTitle = true;
    }

    private async Task SaveTitle()
    {
        if (!string.IsNullOrWhiteSpace(TitleInput))
        {
            List.Title = TitleInput;

            var result = await ListsService.UpdateAsync(
                List.Id, 
                new UpdateListRequest(List.Id, TitleInput));

            if (result.IsSuccess)
            {
                await HomeHubConnection
                    .SendAsync(
                        SendHomeHubConstants.UpdateList , 
                        new UpdateListModel(List.Id, List.Title), 
                        BoardInfo.Id);
            }
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
            await JsRuntime.InvokeVoidAsync("attachClosePopup", DotNetRef, List.Id);
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
        
        SelectedPosition = List.Position;
        Positions = Enumerable.Range(1, BoardInfo.Lists.Count)
            .ToList();
        
        IsMoveMode = true;
    }

    private async Task SubmitMove()
    {
        await OnMoveList.InvokeAsync(
            new MoveListModel(List.Id, SelectedPosition)
        );
        
        IsMoveMode = false;
    }

    private void CancelMove()
    {
        IsMoveMode = false;
    }
    
    private void OpenDeleteConfirm()
    {
        IsDeleteConfirmOpen = true;
    }

    private void CancelDelete()
    {
        IsDeleteConfirmOpen = false;
    }
    
    private async Task ConfirmDelete()
    {
        IsDeleteConfirmOpen = false;
        
        var result = await ListsService.DeleteAsync(List.Id);

        if (result.IsSuccess)
        {
            await OnDeleteList.InvokeAsync(List);
            
            await HomeHubConnection
                .SendAsync(SendHomeHubConstants.DeleteList , List.Id, BoardInfo.Id);
        }
    }
}