using Reenbit.Camp.Frontend.Models.Auth.APIModels;

namespace Reenbit.Camp.Frontend.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ApiClient _apiClient;

    public AuthService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    public async Task<(TokensResponse? Data, string? ErrorMessage)> LoginAsync(LoginRequest request)
    {
        var (data, error) = await _apiClient.PostAsync<LoginRequest, TokensResponse>(
            "auth/login", request);

        return (data, error?.Detail);
    }

    public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterRequest request)
    {
        var (data, error) = await _apiClient.PostAsync<RegisterRequest, object>(
            "auth/register", request);

        if (error != null)
            return (false, error.Detail);

        return (true, null);
    }
}