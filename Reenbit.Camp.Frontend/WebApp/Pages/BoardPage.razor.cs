using Domain.Models.BoardMembers;
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
    
    [Inject] 
    public IBoardMembersService BoardMemberService { get; set; } = default!;

    private BoardInfoModel Board = new();

    private BoardMemberModel CurrentBoardMember = new();

    private bool isLoading;
    
    private bool IsEditingTitle;
    
    private bool IsMembersDialogOpen;
    
    private string TitleInput = string.Empty;
    
    private void OpenMembersDialog()
    {
        IsMembersDialogOpen = true;
    }
    
    private void CloseMembersDialog()
    {
        IsMembersDialogOpen = false;
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
        var boardResult = await BoardsService.GetInfoAsync(BoardId);

        if (boardResult.IsSuccess)
        {
            Board = boardResult.Value;
        }
        
        var memberResult = await BoardMemberService.GeCurrentAsync(BoardId);
        
        if (memberResult.IsSuccess)
        {
            CurrentBoardMember = memberResult.Value;
        }
        
        isLoading = false;
    }
}