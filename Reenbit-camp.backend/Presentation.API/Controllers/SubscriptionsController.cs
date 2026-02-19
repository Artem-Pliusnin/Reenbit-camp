using System.Security.Claims;
using Application.Subscriptions.Commands.CancelSubscription;
using Application.Subscriptions.Commands.CreateCheckoutSession;
using Application.Subscriptions.Queries.GetSubscriptionPlans;
using Application.Subscriptions.Queries.GetUserSubscription;
using Domain.DTOs.Comments;
using Domain.DTOs.Subscriptions;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.Subscriptions;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class SubscriptionsController : AuthorizedContoller
{
    public SubscriptionsController(ISender sender) 
        : base(sender)
    {
    }
    
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUserSubscriptionAsync(
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new GetUserSubscriptionQuery(userId);
        
        Result<UserSubscriptionDto> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("plans")]
    public async Task<IActionResult> GetSubscriptionPlansAsync(
        CancellationToken cancellationToken)
    {
        var query = new GetSubscriptionPlansQuery();
        
        Result<List<SubscriptionPlanDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost("create-checkout")]
    public async Task<IActionResult> CreateCheckoutSessionAsync(
        [FromBody] CreateCheckoutRequest request)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var command = new CreateCheckoutSessionCommand(
            userId,
            request.PlanId,
            request.SuccessUrl,
            request.CancelUrl);

        var result = await Sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
    
    [HttpPost("cancel")]
    public async Task<IActionResult> CancelSubscriptionAsync()
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var result = await Sender.Send(new CancelSubscriptionCommand(userId));

        if (result.IsFailure)
        {
            return BadRequest(result.Error);

        }

        return Ok();
    }

}