using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Services.Abstractions.Services;
using Services.API;

namespace Services.Handlers;

public class TokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly IAuthService _authService;
    private readonly NavigationManager _navigationManager;

    public TokenHandler(
        ILocalStorageService localStorage,
        NavigationManager navigationManager,
        IAuthService authService)
    {
        _localStorage = localStorage;
        _authService = authService;
        _navigationManager = navigationManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var session = await _localStorage
            .GetItemAsync<TokensResponse>("Session");
        
        var now = DateTime.UtcNow;
        
        if (session != null &&
            (IsAccessTokenExpiring(session, now) ||
             IsRefreshTokenExpiring(session, now)))
        {
            var result = await _authService
                .RefreshTokensAsync(new RefreshRequest(session.RefreshToken));

            if (result.IsSuccess)
            {
                await _localStorage.SetItemAsync("Session", result.Value);
                session = result.Value;
            }
            else
            {
                await _localStorage.RemoveItemAsync("Session");
                _navigationManager.NavigateTo("/auth");
            }
        }
        
        if (session != null)
        {
            request.Headers.Authorization = 
                new AuthenticationHeaderValue("Bearer", session.AccessToken);
        }
        else
        {
            _navigationManager.NavigateTo("/auth");
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private static bool IsAccessTokenExpiring(TokensResponse session, DateTime now)
        => (session.AccessTokenExpiresAt - now).TotalSeconds <= 
           (session.AccessTokenExpiresAt - now).TotalSeconds / 2;

    private static bool IsRefreshTokenExpiring(TokensResponse session, DateTime now)
        => (session.RefreshTokenExpiresAt - now).TotalSeconds <= 
           (session.RefreshTokenExpiresAt - now).TotalSeconds / 2;
}
