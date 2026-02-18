using Domain.Enums;
using Domain.Errors;
using Domain.Models;
using Domain.Shared;
using Stripe;
using Stripe.Checkout;

namespace Infrastructure.Helpers;

public static class StripeHelper
{
    public static async Task<Result<CheckoutSessionModel>> GetSessionInfoAsync(Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;

        if (session == null)
        {
            return Result.Failure<CheckoutSessionModel>(SubscriptionErrors.InvalidWebhookPayload);
        }

        try
        {
            var subscriptionService = new SubscriptionService();
            var stripeSubscription = await subscriptionService
                .GetAsync(session.SubscriptionId);

            var sessionInfo = new CheckoutSessionModel(
                int.Parse(session.Metadata["user_id"]),
                int.Parse(session.Metadata["plan_id"]),
                session.SubscriptionId,
                stripeSubscription.Items.Data[0].CurrentPeriodEnd
            );

            return sessionInfo;
        }
        catch
        {
            return Result.Failure<CheckoutSessionModel>(SubscriptionErrors.InvalidWebhookPayload);
        }
    }
    
    public static Result<SubscriptionInfoModel> GetSubscriptionInfo(Event stripeEvent)
    {
        var subscription = stripeEvent.Data.Object as Subscription;

        if (subscription == null)
        {
            return Result.Failure<SubscriptionInfoModel>(SubscriptionErrors.InvalidWebhookPayload);
        }

        var subscriptionInfo = new SubscriptionInfoModel(
            subscription.Id,
            MapStripeStatus(subscription.Status),
            subscription.Items.Data[0].CurrentPeriodEnd,
            subscription.CancelAtPeriodEnd
        );
        
        return subscriptionInfo;
    }
    
    private static SubscriptionStatus MapStripeStatus(string stripeStatus) =>
        stripeStatus switch
        {
            "active" => SubscriptionStatus.Active,
            "past_due" => SubscriptionStatus.PastDue,
            "canceled" => SubscriptionStatus.Canceled,
            "unpaid" => SubscriptionStatus.Unpaid,
            _ => SubscriptionStatus.Canceled
        };
}