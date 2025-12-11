using Refit;
using Domain.Requests.Auth;
using Domain.Responses.Auth;

namespace Services.API;

public interface IAuthApi
{
    [Post("/auth/login")]
    Task<ApiResponse<TokensResponse>> LoginAsync([Body] LoginRequest request);

    [Post("/auth/register")]
    Task<ApiResponse<bool>> RegisterAsync([Body] RegisterRequest request);

    [Post("/auth/logout")]
    Task<ApiResponse<object>> LogoutAsync([Body] LogoutRequest request);

    [Post("/auth/refresh")]
    Task<ApiResponse<TokensResponse>> RefreshAsync([Body] RefreshRequest request);
}