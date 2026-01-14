using Blazored.LocalStorage;
using Domain.Responses.Auth;
using Services.Abstractions.Services;

namespace Services.Authentication;

public class LocalStorageTokenProvider : ITokenProvider
{
    private readonly ILocalStorageService _localStorage;

    public LocalStorageTokenProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        var session =
            await _localStorage.GetItemAsync<TokensResponseDto>("Session");

        return session?.AccessToken;
    }
}