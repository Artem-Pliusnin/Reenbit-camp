using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Subscriptions.Commands.SubscriptionUpdated;

public sealed record SubscriptionUpdatedCommand(
    string SubscriptionId, 
    SubscriptionStatus SubscriptionStatus,  
    DateTime? SubscriptionPeriodEnd,
    bool CancelAtPeriodEnd) : ICommand;