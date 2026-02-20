using Application.Abstractions.Messaging;

namespace Application.Subscriptions.Commands.CheckoutSessionCompleted;

public sealed record CheckoutSessionCompletedCommand(
    int UserId, 
    int PlanId, 
    string SubscriptionId, 
    DateTime? SubscriptionPeriodEnd) 
    : ICommand;