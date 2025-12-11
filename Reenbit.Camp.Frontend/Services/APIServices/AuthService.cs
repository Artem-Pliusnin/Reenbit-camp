using Blazored.LocalStorage;
using Services.Abstractions.Services;
using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Domain.Shared;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;
    private readonly ILocalStorageService _localStorage;

    public AuthService(IAuthApi authApi, 
        ILocalStorageService localStorage)
    {
        _authApi = authApi;
        _localStorage = localStorage;
    }
    
    public async Task<Result<TokensResponse>> LoginAsync(LoginRequest request)
    {
        var response = await _authApi.LoginAsync(request);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<TokensResponse>(error);
    }
    
    public async Task<Result> LogOutAsync()
    {
        var session  = await _localStorage
            .GetItemAsync<TokensResponse>("Session");

        if (session != null && !string.IsNullOrEmpty(session.RefreshToken))
        {
            LogoutRequest request = new LogoutRequest(session.RefreshToken);

            var response =  await _authApi.LogoutAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }
            
            return Result.Failure(response.GetApiErrorAsync());
        }

        return Result.Success();
    }
    
    public async Task<Result<bool>> RegisterAsync(RegisterRequest request)
    {
        var response = await _authApi.RegisterAsync(request);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<bool>(error);
    }
    
    public async Task<Result<TokensResponse>> RefreshTokensAsync(RefreshRequest request)
    {
        var response = await _authApi.RefreshAsync(request);
        
        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<TokensResponse>(error);
    }
}
