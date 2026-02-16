namespace Application.Abstractions.Services;

public interface IStripeService
{
    Task<string> CreateCustomerAsync(
        string email,
        string name,
        CancellationToken cancellationToken = default);

    Task<string> CreateCheckoutSessionAsync(
        string customerId,
        string priceId,
        string successUrl,
        string cancelUrl,
        Dictionary<string, string> metadata,
        CancellationToken cancellationToken = default);

    Task CancelSubscriptionAsync(
        string subscriptionId,
        CancellationToken cancellationToken = default);
}