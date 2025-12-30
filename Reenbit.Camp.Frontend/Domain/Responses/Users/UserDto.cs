namespace Domain.Responses.Users;

public sealed record UserDto(
    int Id,
    string UserName,
    string? Avatar,  
    string Email);