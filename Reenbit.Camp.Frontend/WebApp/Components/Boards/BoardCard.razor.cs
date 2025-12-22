using Domain.Models.Boards;
using Microsoft.AspNetCore.Components;

namespace WebApp.Components.Boards;

public partial class BoardCard : ComponentBase
{
    [Parameter]
    public BoardModel Board { get; set; }
    
    [Parameter]
    public bool IsNew { get; set; }
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    
    private void GoToBoard()
    {
        Navigation.NavigateTo($"/board/{Board.Id}");
    }
}