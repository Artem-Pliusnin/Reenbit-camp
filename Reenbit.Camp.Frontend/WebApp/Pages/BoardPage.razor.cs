using Domain.Models.Boards;
using Domain.Requests.Boards;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services.Abstractions.Services;

namespace WebApp.Pages;

public partial class BoardPage : ComponentBase
{
    [Parameter]
    public int BoardId { get; set; }
    
    [Inject] 
    public IBoardsService BoardsService { get; set; } = default!;

    private BoardInfoModel Board = new();

    private bool isLoading;
    
    private bool IsEditingTitle;
    
    private bool IsMembersDialogOpen;
    
    private string TitleInput = string.Empty;
    
    private void OpenMembersDialog()
    {
        IsMembersDialogOpen = true;
    }

    private void StartEditTitle()
    {
        TitleInput = Board.Title;
        IsEditingTitle = true;
    }

    private async Task SaveTitle()
    {
        if (!string.IsNullOrWhiteSpace(TitleInput))
        {
            Board.Title = TitleInput;

            await BoardsService.UpdateAsync(
                BoardId, 
                new UpdateBoardRequest(BoardId, TitleInput));
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


    protected override async Task OnParametersSetAsync()
    {
        isLoading = true;
        var result = await BoardsService.GetInfoAsync(BoardId);

        if (result.IsSuccess)
        {
            Board = result.Value;
            isLoading = false;
        }
    }
}