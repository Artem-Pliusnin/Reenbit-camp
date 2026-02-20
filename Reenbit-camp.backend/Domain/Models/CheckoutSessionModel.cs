namespace Domain.Models;

public sealed record CheckoutSessionModel(
    int UserId, 
    int PlanId, 
    string SubscriptionId, 
    DateTime? SubscriptionPeriodEnd);