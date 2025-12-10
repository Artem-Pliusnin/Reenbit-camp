using System.Net.Http.Json;
using Reenbit.Camp.Frontend.Models.Shared;

namespace Reenbit.Camp.Frontend.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(TResponse? Data, ApiError? Error)> GetAsync<TResponse>(string url)
    {
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<TResponse>();
            return (data, null);
        }

        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        return (default, error);
    }
    
    
    public async Task<(TResponse? Data, ApiError? Error)> PostAsync<TRequest, TResponse>(string url, TRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(url, request);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<TResponse>();
            return (data, null);
        }
        
        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        return (default, error);
    }
}