namespace Presentation.API.Contracts.Subscriptions;

public sealed record CreateCheckoutRequest(
    int PlanId,
    string SuccessUrl,
    string CancelUrl);