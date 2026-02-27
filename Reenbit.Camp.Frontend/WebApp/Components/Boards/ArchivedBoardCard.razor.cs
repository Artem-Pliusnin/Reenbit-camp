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

    [Inject] 
    public IBoardsService BoardsService { get; set; } = default;
    
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public EventCallback<BoardModel> OnRestoreBoard { get; set; }
    
    private bool IsDialogOpen = false;
    
    private async Task Restore()
    {
        var checkResult = await BoardsService.CanCreateBoardAsync();
        
        if (checkResult.IsFailure || !checkResult.Value)
        {
            IsDialogOpen = true;
            return;
        }
        
        var result = await BoardsService.RestoreBoard(Board.Id);

        if (result.IsSuccess)
        {
            await OnRestoreBoard.InvokeAsync(Board);
        }
    }
    
    private void NavigateToPlans()
    {
        Navigation.NavigateTo("/subscription/plans");
    }
}