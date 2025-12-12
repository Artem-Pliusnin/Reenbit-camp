using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Authentication;
using Domain.Models.Auth;
using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Services.Abstractions.Services;

namespace WebApp.Components.Auth;

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
        
        var result = 
            await AuthService.LoginAsync(new LoginRequest(model.Email, model.Password));
        
        if (!result.IsSuccess)
        {
            ErrorMessage = result.Error?.Detail;
            return;
        }

        if (result.Value?.AccessToken != null && result.Value?.AccessToken != null)
        {
            await ((JwtAuthStateProvider)AuthenticationStateProvider)
                .MarkUserAsLoggedInAsync(result.Value);
            
            Navigation.NavigateTo("/");   
        }
    }
}