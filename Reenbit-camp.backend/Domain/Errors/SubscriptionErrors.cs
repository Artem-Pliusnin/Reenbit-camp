using System.Runtime.InteropServices.JavaScript;
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

    public static readonly Error AlreadyOnThisPlan = new(
        "Subscription.AlreadyOnThisPlan",
        "Customer is already on this plan.");
    
    public static readonly Error InvalidWebhookSignature = new(
        "Subscription.InvalidWebhookSignature",
        "Webhook signature validation failed");
    
    public static readonly Error InvalidWebhookPayload = new(
        "Subscription.InvalidWebhookPayload",
        "Invalid webhook payload");
    
    public static readonly Error BoardsLimitReached = new(
        "Subscription.BoardsLimitReached",
        "User reached the limit of board.");
    
    public static readonly Error UpgradeFailed = new(
        "Subscription.UpgradeFailed",
        "Failed to upgrade the subscription.");
}