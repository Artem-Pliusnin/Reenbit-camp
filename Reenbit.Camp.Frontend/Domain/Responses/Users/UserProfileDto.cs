using Domain.Responses.Subscriptions;
using Domain.Responses.UserAvatars;

namespace Domain.Responses.Users;

public record UserProfileDto(
    int Id,
    string FirstName,
    string LastName,
    string Email, 
    bool HasPassword,
    UserAvatarDto? Avatar,
    UserSubscriptionDto Subscription);