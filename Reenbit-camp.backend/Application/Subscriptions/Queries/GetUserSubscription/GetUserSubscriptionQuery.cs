using Application.Abstractions.Messaging;
using Domain.DTOs.Subscriptions;
using Domain.Entities;

namespace Application.Subscriptions.Queries.GetUserSubscription;

public sealed record GetUserSubscriptionQuery(int UserId) : IQuery<UserSubscriptionDto>;