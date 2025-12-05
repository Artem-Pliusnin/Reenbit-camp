namespace Domain.DTOs.Authorization;

public class LoginResponseDto
{
    public required string AccessToken { get; set; }
    
    public required string RefreshToken { get; set; }
    
    public required DateTime AccessTokenExpiresAt { get; set; }

    public required DateTime RefreshTokenExpiresAt { get; set; }
    
}