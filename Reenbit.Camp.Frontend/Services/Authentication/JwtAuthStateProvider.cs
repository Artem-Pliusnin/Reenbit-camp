using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Domain.Requests.Auth;
using Domain.Responses.Auth;
using Services.Abstractions.Services;

namespace Services.Authentication;

public class JwtAuthStateProvider : AuthenticationStateProvider 
{
    private ILocalStorageService _localStorage;
    private IAuthService _authService;
    
    public JwtAuthStateProvider(
        ILocalStorageService localStorage,
        IAuthService authService)
    {
        _localStorage = localStorage;
        _authService = authService;
    }
    
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var session  = await _localStorage
            .GetItemAsync<TokensResponseDto>("Session");
        
        var identity = new ClaimsIdentity();
        
        if (session != null && !string.IsNullOrEmpty(session.AccessToken))
        {
            if (session.AccessTokenExpiresAt <= DateTime.UtcNow)
            {
                var result = await _authService
                    .RefreshTokensAsync(new RefreshRequest(session.RefreshToken));

                if (result.IsSuccess)
                {
                    await _localStorage.SetItemAsync("Session", result.Value);
                    identity = GetClaimsIdentity(result.Value.AccessToken);
                }
            }
            else
            {
                identity = GetClaimsIdentity(session.AccessToken);
            }
        }

        var user = new ClaimsPrincipal(identity);
        
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsLoggedInAsync(TokensResponseDto responseDto)
    {
        await _localStorage.SetItemAsync("Session", responseDto);
        
        var identity = GetClaimsIdentity(responseDto.AccessToken);
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