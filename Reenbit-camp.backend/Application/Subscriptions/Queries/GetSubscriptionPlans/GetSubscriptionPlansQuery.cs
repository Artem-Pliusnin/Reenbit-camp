using Application.Abstractions.Messaging;
using Domain.DTOs.Subscriptions;

namespace Application.Subscriptions.Queries.GetSubscriptionPlans;

public sealed record GetSubscriptionPlansQuery() : IQuery<List<SubscriptionPlanDto>>;