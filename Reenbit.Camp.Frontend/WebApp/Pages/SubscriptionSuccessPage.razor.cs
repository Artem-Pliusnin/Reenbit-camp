using Microsoft.AspNetCore.Components;

namespace WebApp.Pages;

public partial class SubscriptionSuccessPage : ComponentBase
{
    [Inject] 
    public NavigationManager Navigation { get; set; } = default!;

    private void GoToBoards()
    {
        Navigation.NavigateTo("/");
    }
}