using Domain.Shared;

namespace Domain.Errors;

public static class SubscriptionErrors
{
    public static readonly Error UserDataNotFound = new(
        "Subscription.UserDataNotFound",
        "User's subscription information not found.");
    
    public static readonly Error PlanNotFound = new(
        "Subscription.PlanNotFound",
        "Subscription plan doesn't exist.");
}