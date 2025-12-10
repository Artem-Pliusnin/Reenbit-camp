using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Reenbit.Camp.Frontend.Models.Auth.APIModels;

namespace Reenbit.Camp.Frontend.Handlers;

public class TokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public TokenHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        var session = await _localStorage.GetItemAsync<TokensResponse>("Session");

        if (session != null && !string.IsNullOrEmpty(session.AccessToken))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", session.AccessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}