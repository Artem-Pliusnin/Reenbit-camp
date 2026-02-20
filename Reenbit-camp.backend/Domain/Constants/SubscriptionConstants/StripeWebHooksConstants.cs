namespace Domain.Constants.SubscriptionConstants;

public static class StripeWebHooksConstants
{
    public const string SessionCompleted = "checkout.session.completed";
    public const string SubscriptionUpdated = "customer.subscription.updated";
    public const string SubscriptionDeleted = "customer.subscription.deleted";
}