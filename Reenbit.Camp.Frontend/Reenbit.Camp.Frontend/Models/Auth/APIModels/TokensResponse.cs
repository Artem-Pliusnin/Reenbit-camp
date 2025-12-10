namespace Reenbit.Camp.Frontend.Models.Auth.APIModels;

public sealed record TokensResponse(
    string AccessToken, 
    string RefreshToken, 
    DateTime AccessTokenExpiresAt, 
    DateTime RefreshTokenExpiresAt);