using Microsoft.AspNetCore.Components;
using Domain.Models.Auth;
using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Services.Abstractions.Services;

namespace WebApp.Components.Auth;

public partial class Register : ComponentBase
{
    private RegisterModel model = new();
    
    private string? ErrorMessage { get; set; }
    
    [Inject] 
    public IAuthService AuthService { get; set; } = default!;
    
    [Parameter]
    public EventCallback OnChangeToLogin { get; set; }
    
    private async Task OnRegisterAsync()
    {
        ErrorMessage = null;
        
        var result = 
            await AuthService.RegisterAsync(
                new RegisterRequest(
                    model.FirstName, 
                    model.LastName,
                    model.Email, 
                    model.Password));
        
        if (!result.IsSuccess)
        {
            ErrorMessage = result.Error?.Detail;
            return;
        }
        
        await OnChangeToLogin.InvokeAsync(null);
    }
}