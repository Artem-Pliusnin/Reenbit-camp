using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Subscriptions.Queries.HasMinimumPlan;

public sealed record HasMinimumPlanQuery(
    int UserId, 
    SubscriptionPlanType MinimumPlan) 
    : IQuery<bool>;