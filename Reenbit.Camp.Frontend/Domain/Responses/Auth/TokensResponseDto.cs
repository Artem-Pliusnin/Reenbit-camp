namespace Domain.Responses.Auth;

public sealed record TokensResponseDto(
    string AccessToken, 
    string RefreshToken, 
    DateTime AccessTokenExpiresAt, 
    DateTime RefreshTokenExpiresAt);