namespace Domain.Responses.Subscriptions;

public sealed record UserSubscriptionDto(
    int Id,
    DateTime? CurrentPeriodEnd,
    bool CancelAtPeriodEnd,
    SubscriptionPlanDto SubscriptionPlan);