using Domain.Shared;
using Refit;

namespace Services.Extensions;

public static class ErrorExtensions
{
    public static ApiError? GetApiErrorAsync<T>(this ApiResponse<T> response)
    {
        if (response.IsSuccessStatusCode)
        {
            return null;
        }

        if (response.Error?.Content == null)
            return new ApiError
            {
                Title = "Network Error",
                Detail = "No response from server",
                Status = 0
            };

        try
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiError = System.Text.Json.JsonSerializer
                .Deserialize<ApiError>(response.Error.Content, options);
            
            apiError ??= new ApiError();
            apiError.Status ??= (int)response.StatusCode;

            return apiError;
        }
        catch
        {
            return new ApiError
            {
                Title = "Deserialization Error",
                Detail = "Failed to parse error response",
                Status = (int?)response.StatusCode
            };
        }
    }
}