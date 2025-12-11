using Blazored.LocalStorage;
using Services.Abstractions.Services;
using Domain.APIModels.Auth;

namespace Services.APIServices;

public class AuthService : IAuthService
{
    private readonly ApiClient _apiClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(ApiClient apiClient, 
        ILocalStorageService localStorage)
    {
        _apiClient = apiClient;
        _localStorage = localStorage;
    }
    
    public async Task<(TokensResponse? Data, string? ErrorMessage)> LoginAsync(LoginRequest request)
    {
        var (data, error) = await _apiClient.PostAsync<LoginRequest, TokensResponse>(
            "auth/login", request);

        return (data, error?.Detail);
    }
    
    public async Task LogOutAsync()
    {
        var session  = await _localStorage
            .GetItemAsync<TokensResponse>("Session");

        if (session != null && !string.IsNullOrEmpty(session.RefreshToken))
        {
            LogoutRequest request = new LogoutRequest(session.RefreshToken);

            await _apiClient.PostAsyncRaw("auth/logout", request);
        }
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
