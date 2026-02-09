using Domain.Models.Boards;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Components.Boards;

public partial class ArchivedBoardCard : ComponentBase
{
    [Parameter]
    public BoardModel Board { get; set; }
    
    [Parameter]
    public bool IsNew { get; set; }

    [Inject] public IBoardsService BoardsService { get; set; } = default;
    
    [Parameter, EditorRequired]
    public EventCallback<BoardModel> OnRestoreBoard { get; set; }
    
    
    private async Task Restore()
    {
        var result = await BoardsService.RestoreBoard(Board.Id);

        if (result.IsSuccess)
        {
            await OnRestoreBoard.InvokeAsync(Board);
        }
    }
}