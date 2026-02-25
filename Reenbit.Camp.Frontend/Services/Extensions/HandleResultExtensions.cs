using System.Text.Json;
using Domain.Shared;
using Refit;

namespace Services.Extensions;

public static class HandleResultExtensions
{
    public static Result<T> HandleResult<T>(
        this ApiResponse<T> response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Result.Success(response.Content);
        }

        var error = response.GetApiErrorAsync();
        return Result.Failure<T>(error);
    }
    
    public static Result<TResult> HandleResultWithMapping<TResponse, TResult>(
        this ApiResponse<TResponse> response,
        Func<TResponse, TResult> mapFunc)
    {
        if (response.IsSuccessStatusCode)
        {
            TResult mapped = default;
            if (response.Content != null)
            {
                mapped = mapFunc(response.Content);
            }
            
            return Result.Success<TResult>(mapped);
        }

        var error = response.GetApiErrorAsync(); 
        return Result.Failure<TResult>(error);
    }
}