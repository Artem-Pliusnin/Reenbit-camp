using Domain.DTOs.Subscriptions;
using Domain.DTOs.UserAvatars;

namespace Domain.DTOs.Users;

public class UserDto
{
    public required int Id { get; set; }
    
    public required string UserName { get; set; }

    public required string Email { get; set; }
    
    public required UserAvatarDto? Avatar { get; set; }
    
    public required UserSubscriptionDto Subscription { get; set; }
}