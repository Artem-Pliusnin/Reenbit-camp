using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Subscriptions.Commands.ResumeSubscription;

public sealed record ResumeSubscriptionCommand(int UserId) : ICommand;