using Reenbit.Camp.Frontend.Models.Auth.APIModels;

namespace Reenbit.Camp.Frontend.Services.Refresh;

public interface IRefreshService
{
    Task<(TokensResponse? Data, string? ErrorMessage)> RefreshTokensAsync(RefreshRequest request);
}