using System.Timers;
using Domain.Models.Boards;
using Domain.Requests.Boards;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services.Abstractions.Services;

namespace WebApp.Pages;

public partial class ArchivePage : ComponentBase, IDisposable
{
    private ArchivedBoardsFilter filter = new();
    
    private bool isLoading = false;
    
    [Inject] 
    public IBoardsService BoardsService { get; set; } = default!;

    private List<BoardModel> Boards = new List<BoardModel>();
    
    private System.Timers.Timer aTimer = default!;

    private int TotalPages;
    
    private IEnumerable<int> VisiblePages
    {
        get
        {
            const int maxPagesToShow = 5;

            var start = Math.Max(1, filter.Page - 2);
            var end = Math.Min(TotalPages, start + maxPagesToShow - 1);
            
            start = Math.Max(1, end - maxPagesToShow + 1);

            return Enumerable.Range(start, end - start + 1);
        }
    }
    
    protected override async Task OnInitializedAsync()
    {
        await LoadBoardsAsync();
        aTimer = new System.Timers.Timer(300);
        aTimer.Elapsed += OnTimerElapsed;
        aTimer.AutoReset = false;
    }

    private void ResetTimer(KeyboardEventArgs e)
    {
        aTimer.Stop();
        aTimer.Start();
    }
    
    private async void OnTimerElapsed(Object? source, ElapsedEventArgs e)
    {
        await LoadBoardsAsync();
    }

    private async void OnOnlyMyBoardsChanged()
    {
        await LoadBoardsAsync();
    }
    
    private async Task LoadBoardsAsync()
    {
        isLoading = true;
        var result = await BoardsService.GetArchivedByUserAsync(filter);

        if (result.IsSuccess)
        {
            Boards = result.Value.Dtos;
            filter.Page =  result.Value.CurrentPage;
            TotalPages = result.Value.TotalPages;
            
            isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }
    
    private async Task ChangePage(int page)
    {
        if (page < 1 || page > TotalPages)
            return;

        filter.Page = page;
        await LoadBoardsAsync();
    }

    private async Task OnRestoreBoard()
    {
        await LoadBoardsAsync();
    }
    
    void IDisposable.Dispose()
        => aTimer?.Dispose(); 

}