using System.Security.Claims;
using Application.Boards.Queries.GetUserBoards;
using Application.Users.Queries.GetInviteSuggestionUsers;
using Domain.DTOs.Boards;
using Domain.DTOs.Shared;
using Domain.DTOs.Users;
using Domain.Errors;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.Users;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class UsersController : AuthorizedContoller
{
    public UsersController(ISender sender)
        : base(sender)
    {}
    
    [HttpGet("invitation/suggestions")]
    public async Task<IActionResult> GetInvitationSuggestionsAsync(
        [FromQuery] GetInviteSuggestionRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new GetInviteSuggestionUsersQuery(
            request.BoardId,
            userId,
            request.Query, 
            request.Limit);
        
        Result<List<UserDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
}