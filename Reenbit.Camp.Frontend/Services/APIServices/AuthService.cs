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
    
    public async Task<Result<TokensResponseDto>> LoginAsync(LoginRequest request)
    {
        var response = await _authApi.LoginAsync(request);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<TokensResponseDto>(error);
    }
    
    public async Task<Result> LogOutAsync()
    {
        var session = await _localStorage.GetItemAsync<TokensResponseDto>("Session");
        
        if (session == null || string.IsNullOrEmpty(session.AccessToken))
        {
            return Result.Failure(ApiError.NullValue);
        }

        if (session.AccessTokenExpiresAt < DateTime.UtcNow)
        {
            await _localStorage.RemoveItemAsync("Session");
            return Result.Success();
        }
        
        var response =  await _authApi.LogoutAsync(session.AccessToken);
            
        if (response.IsSuccessStatusCode) 
        {
            return Result.Success();
        }
            
        return Result.Failure(response.GetApiErrorAsync());
    }

    
    public async Task<Result<bool>> RegisterAsync(RegisterRequest request)
    {
        var response = await _authApi.RegisterAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<bool>(error);
    }
    
    public async Task<Result<TokensResponseDto>> RefreshTokensAsync(RefreshRequest request)
    {
        var response = await _authApi.RefreshAsync(request);
        
        if (response.IsSuccessStatusCode && response.Content != null)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<TokensResponseDto>(error);
    }
}
