using Domain.Enums;
using Domain.Models.Boards;
using Microsoft.AspNetCore.Components;

namespace WebApp.Components.Boards;

public partial class BoardCard : ComponentBase
{
    [Parameter, EditorRequired]
    public BoardCardModel Board { get; set; }
    
    [Parameter, EditorRequired]
    public bool IsNew { get; set; }
    
    [Parameter, EditorRequired]
    public int CurrentUserId { get; set; }
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    
    private void GoToBoard()
    {
        if (Board.Status == BoardStatus.Blocked)
        {
            return;
        }
        
        Navigation.NavigateTo($"/board/{Board.Id}");
    }
}