using Domain.Models.Boards;
using Microsoft.AspNetCore.Components;

namespace WebApp.Components.Boards;

public partial class BoardCard : ComponentBase
{
    [Parameter]
    public BoardModel Board { get; set; }
}