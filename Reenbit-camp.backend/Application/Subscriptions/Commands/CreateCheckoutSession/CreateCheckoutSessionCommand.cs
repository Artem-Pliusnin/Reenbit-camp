using Application.Abstractions.Messaging;

namespace Application.Subscriptions.Commands.CreateCheckoutSession;

public sealed record CreateCheckoutSessionCommand(
    int UserId,
    int SubscriptionPlanId,
    string SuccessUrl,
    string CancelUrl,
    string ChangeUrl
) : ICommand<string>;