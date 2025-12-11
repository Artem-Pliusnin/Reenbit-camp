using Domain.Requests.Auth;
using Domain.Responses.Auth;

namespace Services.Abstractions.Services;

public interface IAuthService
{
    Task<(TokensResponse? Data, string? ErrorMessage)> LoginAsync(LoginRequest request);
    
    Task LogOutAsync();
    
    Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterRequest request);
}