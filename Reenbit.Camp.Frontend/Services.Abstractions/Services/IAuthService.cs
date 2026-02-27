using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IAuthService
{
    Task<Result<TokensResponseDto>> LoginAsync(LoginRequest request);
    
    Task<Result> LogOutAsync();
    
    Task<Result<bool>> RegisterAsync(RegisterRequest request);
    
    Task<Result<TokensResponseDto>> RefreshTokensAsync(RefreshRequest request);
    
    string GenerateGoogleAuthUrl(string returnUrl);
}