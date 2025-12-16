using Refit;
using Domain.Requests.Auth;
using Domain.Responses.Auth;

namespace Services.API;

public interface IAuthApi
{
    [Post("/auth/login")]
    Task<ApiResponse<TokensResponseDto>> LoginAsync([Body] LoginRequest request);

    [Post("/auth/register")]
    Task<ApiResponse<bool>> RegisterAsync([Body] RegisterRequest request);

    [Post("/auth/logout")]
    Task<ApiResponse<object>> LogoutAsync([Header("Authorization")] string bearerToken);

    [Post("/auth/refresh")]
    Task<ApiResponse<TokensResponseDto>> RefreshAsync([Body] RefreshRequest request);
}