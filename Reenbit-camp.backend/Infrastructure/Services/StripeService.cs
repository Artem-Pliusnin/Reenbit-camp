using Application.Abstractions.Services;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Infrastructure.Services;

public class StripeService : IStripeService
{
    private readonly StripeSettings _stripeSettings;

    public StripeService(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
        
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }
    
    public async Task<string> CreateCustomerAsync(
        string email,
        string name,
        CancellationToken cancellationToken = default)
    {
        var options = new CustomerCreateOptions
        {
            Email = email,
            Name = name,
        };

        var service = new CustomerService();
        
        var customer = await service.CreateAsync(
            options, 
            null, 
            cancellationToken);

        return customer.Id;
    }

    public async Task<string> CreateCheckoutSessionAsync(
        string customerId, 
        string priceId, 
        string successUrl, 
        string cancelUrl,
        Dictionary<string, string> metadata, 
        CancellationToken cancellationToken = default)
    {
        var options = new SessionCreateOptions
        {
            Customer = customerId,
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1,
                }
            },
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            Metadata = metadata,
            SubscriptionData = new SessionSubscriptionDataOptions
            {
                Metadata = metadata
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(
            options, 
            null, 
            cancellationToken);

        return session.Url;
    }

    public async Task CancelSubscriptionAsync(
        string subscriptionId, 
        CancellationToken cancellationToken = default)
    {
        var service = new SubscriptionService();
        var options = new SubscriptionUpdateOptions
        {
            CancelAtPeriodEnd = true
        };

        await service.UpdateAsync(
            subscriptionId, 
            options, 
            null, 
            cancellationToken);
    }
    
    public async Task CancelSubscriptionImmediatelyAsync(
        string subscriptionId,
        CancellationToken cancellationToken = default)
    {
        var service = new SubscriptionService();
        
        await service.CancelAsync(
            subscriptionId, 
            cancellationToken: cancellationToken);
    }
    
}