using Application.Subscriptions.Commands.CheckoutSessionCompleted;
using Application.Subscriptions.Commands.SubscriptionDeleted;
using Application.Subscriptions.Commands.SubscriptionUpdated;
using Domain.Constants.SubscriptionConstants;
using Domain.Errors;
using Domain.Shared;
using Infrastructure.Configuration;
using Infrastructure.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.API.Abstractions;
using Stripe;

namespace Presentation.API.Controllers;

[Route("api/webhooks/stripe")]
public class StripeWebhooksController : ApiController
{
    private readonly StripeSettings _stripeSettings;

    public StripeWebhooksController(
        ISender sender, 
        IOptions<StripeSettings> stripeSettings) 
        :base(sender)
    {
        _stripeSettings = stripeSettings.Value;
    }
    
    [HttpPost]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body)
            .ReadToEndAsync();
        
        var signature = Request.Headers["Stripe-Signature"].ToString();
        var webhookSecret = _stripeSettings.WebhookSecret;

        try
        {
            var stripeEvent = EventUtility
                .ConstructEvent(json, signature, webhookSecret);

            switch (stripeEvent.Type)
            {
                case StripeWebHooksConstants.SessionCompleted:
                {
                    return await HandleSessionCompleted(stripeEvent);
                }

                case StripeWebHooksConstants.SubscriptionUpdated:
                {
                    return await HandleSubscriptionUpdated(stripeEvent);
                }

                case StripeWebHooksConstants.SubscriptionDeleted:
                {
                    return await HandleSubscriptionDeleted(stripeEvent);
                }
            }
            
        }
        catch (StripeException ex)
        {
            return HandleFailure(
                Result.Failure(SubscriptionErrors.InvalidWebhookSignature));
        }
        
        return Ok();
    }

    private async Task<IActionResult> HandleSessionCompleted(Event stripeEvent)
    {
        var sesionInfoResult = await StripeHelper
            .GetSessionInfoAsync(stripeEvent);

        if (sesionInfoResult.IsFailure)
        {
            return HandleFailure(sesionInfoResult);
        }

        var command = new CheckoutSessionCompletedCommand(
            sesionInfoResult.Value.UserId,
            sesionInfoResult.Value.PlanId,
            sesionInfoResult.Value.SubscriptionId,
            sesionInfoResult.Value.SubscriptionPeriodEnd);

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    private async Task<IActionResult> HandleSubscriptionUpdated(Event stripeEvent)
    {
        var subscriptionInfoResult = StripeHelper
            .GetSubscriptionInfo(stripeEvent);

        if (subscriptionInfoResult.IsFailure)
        {
            return HandleFailure(subscriptionInfoResult);
        }

        var command = new SubscriptionUpdatedCommand(
            subscriptionInfoResult.Value.SubscriptionId,
            subscriptionInfoResult.Value.SubscriptionStatus,
            subscriptionInfoResult.Value.SubscriptionPeriodEnd,
            subscriptionInfoResult.Value.CancelAtPeriodEnd);

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    private async Task<IActionResult> HandleSubscriptionDeleted(Event stripeEvent)
    {
        var subscriptionInfoResult = StripeHelper
            .GetSubscriptionInfo(stripeEvent);

        if (subscriptionInfoResult.IsFailure)
        {
            return HandleFailure(subscriptionInfoResult);
        }

        var command = new SubscriptionDeletedCommand(
            subscriptionInfoResult.Value.SubscriptionId
        );

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
}