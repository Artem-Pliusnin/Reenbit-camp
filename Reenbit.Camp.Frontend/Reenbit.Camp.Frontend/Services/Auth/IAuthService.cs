using Reenbit.Camp.Frontend.Models.Auth.APIModels;

namespace Reenbit.Camp.Frontend.Services.Auth;

public interface IAuthService
{
    Task<(TokensResponse? Data, string? ErrorMessage)> LoginAsync(LoginRequest request);
    
    Task LogOutAsync();
    
    Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterRequest request);
}