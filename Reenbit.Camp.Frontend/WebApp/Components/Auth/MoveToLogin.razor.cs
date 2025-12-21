using Microsoft.AspNetCore.Components;

namespace WebApp.Components.Auth;

public partial class MoveToLogin : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    protected override void OnInitialized()
    {
        NavigationManager.NavigateTo("/auth");
    }
}