using Domain.APIModels.Auth;

namespace Services.Abstractions.Services;

public interface IRefreshService
{
    Task<(TokensResponse? Data, string? ErrorMessage)> RefreshTokensAsync(RefreshRequest request);
}