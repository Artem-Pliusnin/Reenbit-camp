using Domain.Models.Boards;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Pages;

public partial class BoardPage : ComponentBase
{
    [Parameter]
    public int BoardId { get; set; }
    
    [Inject] 
    public IBoardsService BoardsService { get; set; } = default!;

    private BoardInfoModel Board = new();

    private bool isLoading = false;

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