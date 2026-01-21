using Domain.Responses.UserAvatars;

namespace Domain.Responses.Users;

public record UserProfileDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    UserAvatarDto? Avatar);