using System.Security.Claims;
using Application.Subscriptions.Queries.HasMinimumPlan;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireSubscriptionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly SubscriptionPlanType _minimumPlanType;

    public RequireSubscriptionAttribute(SubscriptionPlanType minimumPlanType)
    {
        _minimumPlanType = minimumPlanType;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userIdClaim = context.HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var sender = context.HttpContext.RequestServices
            .GetRequiredService<ISender>();
        
        var query = new HasMinimumPlanQuery(userId, _minimumPlanType);
        
        var result = await sender.Send(query);
        
        if (result.IsFailure)
        {
            context.Result = new ObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            })
            {
                StatusCode = 403
            };
            return;
        }
        
        if (!result.Value)
        {
            context.Result = new ObjectResult(new
            {
                error = "Subscription.InsufficientPlan",
                message = $"This feature requires {_minimumPlanType} plan or higher"
            })
            {
                StatusCode = 403
            };
        }
    }
}