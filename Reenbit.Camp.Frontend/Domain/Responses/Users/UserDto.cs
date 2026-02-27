using Domain.Responses.Subscriptions;
using Domain.Responses.UserAvatars;

namespace Domain.Responses.Users;

public sealed record UserDto(
    int Id,
    string UserName,
    string Email,
    UserAvatarDto? Avatar,
    UserSubscriptionDto Subscription);