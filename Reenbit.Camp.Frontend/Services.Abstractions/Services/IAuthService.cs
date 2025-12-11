using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IAuthService
{
    Task<Result<TokensResponse>> LoginAsync(LoginRequest request);
    
    Task<Result> LogOutAsync();
    
    Task<Result<bool>> RegisterAsync(RegisterRequest request);
    
    Task<Result<TokensResponse>> RefreshTokensAsync(RefreshRequest request);
}