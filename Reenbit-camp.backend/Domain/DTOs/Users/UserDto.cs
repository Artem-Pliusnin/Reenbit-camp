namespace Domain.DTOs.Users;

public class UserDto
{
    public required int Id { get; set; }
    
    public required string UserName { get; set; }
    
    public required string? Avatar { get; set; }

    public required string Email { get; set; }
}