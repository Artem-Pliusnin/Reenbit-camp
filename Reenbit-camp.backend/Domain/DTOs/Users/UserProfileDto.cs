using Domain.DTOs.Subscriptions;
using Domain.DTOs.UserAvatars;

namespace Domain.DTOs.Users;

public class UserProfileDto
{
    public required int Id { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public required string Email { get; set; }
    
    public required bool HasPassword { get; set; }
    
    public required UserAvatarDto? Avatar { get; set; }
    
    public required UserSubscriptionDto Subscription { get; set; }
}