using Microsoft.AspNetCore.Components;
using Reenbit.Camp.Frontend.Models.Auth;
using Reenbit.Camp.Frontend.Models.Auth.APIModels;
using Reenbit.Camp.Frontend.Services.Auth;

namespace Reenbit.Camp.Frontend.Components.Auth;

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
        
        var (data, errorMessage) = 
            await AuthService.RegisterAsync(
                new RegisterRequest(
                    model.FirstName, 
                    model.LastName,
                    model.Email, 
                    model.Password));
        
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ErrorMessage = errorMessage;
            return;
        }
        
        await OnChangeToLogin.InvokeAsync(null);
    }
}