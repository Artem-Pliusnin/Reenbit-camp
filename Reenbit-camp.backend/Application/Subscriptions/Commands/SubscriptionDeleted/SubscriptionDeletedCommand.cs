using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Subscriptions.Commands.SubscriptionDeleted;

public sealed record SubscriptionDeletedCommand(
    string SubscriptionId) 
    : ICommand;