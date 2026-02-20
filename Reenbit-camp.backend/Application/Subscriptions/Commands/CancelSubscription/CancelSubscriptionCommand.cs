using Application.Abstractions.Messaging;

namespace Application.Subscriptions.Commands.CancelSubscription;

public sealed record CancelSubscriptionCommand(int UserId) : ICommand;