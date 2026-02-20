using Application.Abstractions.Messaging;

namespace Application.Subscriptions.Commands.CancelExpiredSubscriptions;

public sealed record CancelExpiredSubscriptionsCommand() : ICommand;