using System.Security.Claims;
using Application.BoardMembers.Queries.HasRequiredRole;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.API.Attributes;

public class RequireBoardRoleAttribute: Attribute, IAsyncAuthorizationFilter
{
    private readonly BoardRole _minimumRole;

    public RequireBoardRoleAttribute(BoardRole minimumRole)
    {
        _minimumRole = minimumRole;
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

        var routeValues = context.HttpContext.Request.RouteValues;

        if (!routeValues.TryGetValue("boardId", out var boardIdObj) ||
            !int.TryParse(boardIdObj?.ToString(), out var boardId))
        {
            context.Result = new ObjectResult(
                CreateProblemDetails(
                    "Bad Request",
                    StatusCodes.Status400BadRequest,
                    BoardErrors.MissingBoardId))
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }

        var sender = context.HttpContext.RequestServices
            .GetRequiredService<ISender>();

        var query = new HasRequiredRoleQuery(boardId, userId, _minimumRole);
        var result = await sender.Send(query);

        if (result.IsFailure)
        {
            context.Result = new ObjectResult(
                CreateProblemDetails(
                    "Forbidden",
                    StatusCodes.Status403Forbidden,
                    result.Error))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        if (!result.Value)
        {
            context.Result = new ObjectResult(
                CreateProblemDetails(
                    "Forbidden",
                    StatusCodes.Status403Forbidden,
                    BoardErrors.InsufficientRole(_minimumRole.ToString())))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
    
    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}