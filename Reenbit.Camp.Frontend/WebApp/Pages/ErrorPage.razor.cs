using Microsoft.AspNetCore.Components;

namespace WebApp.Pages;

public partial class ErrorPage : ComponentBase
{
    [Inject] 
    NavigationManager NavigationManager { get; set; } = default!;

    private void Reload()
    {
        NavigationManager.NavigateTo("/", forceLoad: true);
    }
}