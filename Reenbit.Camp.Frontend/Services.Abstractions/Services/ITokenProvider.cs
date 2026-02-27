namespace Services.Abstractions.Services;

public interface ITokenProvider
{
    Task<string?> GetAccessTokenAsync();
}