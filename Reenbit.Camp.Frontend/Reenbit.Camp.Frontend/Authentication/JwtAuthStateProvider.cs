using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Reenbit.Camp.Frontend.Models.Auth.APIModels;

namespace Reenbit.Camp.Frontend.Authentication;

public class JwtAuthStateProvider : AuthenticationStateProvider 
{
    private ILocalStorageService _localStorage;
    
    public JwtAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var session  = await _localStorage
            .GetItemAsync<TokensResponse>("Session");
        
        var identity = new ClaimsIdentity();
        
        if (session != null && !string.IsNullOrEmpty(session.AccessToken))
        {
            identity = GetClaimsIdentity(session.AccessToken);
        }

        var user = new ClaimsPrincipal(identity);
        
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsLoggedInAsync(TokensResponse response)
    {
        await _localStorage.SetItemAsync("Session", response);
        
        var identity = GetClaimsIdentity(response.AccessToken);
        var user = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        await _localStorage.RemoveItemAsync("Session");
        
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user)));
    }

    private ClaimsIdentity GetClaimsIdentity(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var claims = jwtToken.Claims;
        return new ClaimsIdentity(claims, "jwt");
    }
}