namespace Domain.APIModels.Auth;

public sealed record TokensResponse(
    string AccessToken, 
    string RefreshToken, 
    DateTime AccessTokenExpiresAt, 
    DateTime RefreshTokenExpiresAt);