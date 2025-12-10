using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Reenbit.Camp.Frontend.Authentication;
using Reenbit.Camp.Frontend.Models.Auth.APIModels;
using Reenbit.Camp.Frontend.Services.Auth;
using Reenbit.Camp.Frontend.Services.Refresh;

namespace Reenbit.Camp.Frontend.Handlers;

public class TokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly IRefreshService _refreshService;
    private readonly NavigationManager _navigationManager;

    public TokenHandler(
        ILocalStorageService localStorage,
        NavigationManager navigationManager,
        IRefreshService refreshService)
    {
        _localStorage = localStorage;
        _refreshService = refreshService;
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
            var (newTokens, _) = await _refreshService
                .RefreshTokensAsync(new RefreshRequest(session.RefreshToken));

            if (newTokens != null)
            {
                await _localStorage.SetItemAsync("Session", newTokens);
            }
            else
            {
                await _localStorage.RemoveItemAsync("Session");
                _navigationManager.NavigateTo("/auth");
            }
            session = newTokens;
        }
        
        if (session != null && !string.IsNullOrEmpty(session.AccessToken))
        {
            request.Headers.Authorization = 
                new AuthenticationHeaderValue("Bearer", session.AccessToken);
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
