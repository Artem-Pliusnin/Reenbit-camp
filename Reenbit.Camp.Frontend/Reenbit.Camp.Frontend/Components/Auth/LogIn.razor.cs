using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Reenbit.Camp.Frontend.Authentication;
using Reenbit.Camp.Frontend.Models.Auth;
using Reenbit.Camp.Frontend.Models.Auth.APIModels;
using Reenbit.Camp.Frontend.Services.Auth;

namespace Reenbit.Camp.Frontend.Components.Auth;

public partial class LogIn : ComponentBase
{
    private LoginModel model = new();
    
    private string? ErrorMessage { get; set; }
    
    [Inject] 
    public IAuthService AuthService { get; set; } = default!;
    
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    private async Task OnLogin()
    {
        ErrorMessage = null;
        
        var (data, errorMessage) = 
            await AuthService.LoginAsync(new LoginRequest(model.Email, model.Password));
        
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ErrorMessage = errorMessage;
            return;
        }

        if (data.AccessToken != null && data.RefreshToken != null)
        {
            await ((JwtAuthStateProvider)AuthenticationStateProvider)
                .MarkUserAsLoggedInAsync(data);
            
            Navigation.NavigateTo("/");   
        }
    }
}