using System.Net.Http.Json;
using Services.Abstractions.Services;
using Domain.Requests.Auth;
using Domain.Responses.Auth;

namespace Services.APIServices;

public class RefreshService : IRefreshService
{
    private readonly HttpClient _httpClient;

    public RefreshService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<(TokensResponse? Data, string? ErrorMessage)> RefreshTokensAsync(RefreshRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/refresh", request);
        if (!response.IsSuccessStatusCode)
            return (null, "Refresh failed");

        var data = await response.Content.ReadFromJsonAsync<TokensResponse>();
        return (data, null);
    }
}