using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Reenbit.Camp.Frontend.Authentication;
using Reenbit.Camp.Frontend.Models.Auth.APIModels;
using Reenbit.Camp.Frontend.Services.Auth;

namespace Reenbit.Camp.Frontend.Components.Auth;

public partial class Logout : ComponentBase
{
    [Inject]
    public IAuthService AuthService { get; set; }
    
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;
    
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private async Task OnLogOut()
    {
        await AuthService.LogOutAsync();
        
        await ((JwtAuthStateProvider)AuthenticationStateProvider)
            .MarkUserAsLoggedOutAsync();
        
        Navigation.NavigateTo("auth");
    }
}