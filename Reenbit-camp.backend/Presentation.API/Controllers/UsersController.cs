using System.Security.Claims;
using Application.Boards.Queries.GetUserBoards;
using Application.CardMembers.Queries.GetNotConnectedToCard;
using Application.Users.Commands.UpdateUserAvatar;
using Application.Users.Commands.UpdateUserInfo;
using Application.Users.Queries.GetInviteSuggestionUsers;
using Application.Users.Queries.GetUserInfo;
using Application.Users.Queries.GetUserProfileInfo;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfoAsync(
        [FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var query = new GetUserInfoQuery(id);
        
        var result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("{id}/profile")]
    public async Task<IActionResult> GetUserProfileAsync(
        [FromRoute] int id, 
        CancellationToken cancellationToken)
    {
        var query = new GetUserProfileInfoQuery(id);
        
        var result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserInfoAsync(
        [FromRoute] int id,
        [FromBody] UpdateUserInfoRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId)){
        
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        if (id != userId)
        {
            return HandleFailure(
                Result.Failure(PermissionErrors.InsufficientPermissions));
        }
        
        var command = new UpdateUserInfoCommand(
            userId,
            request.FirstName,
            request.LastName);
        
        var result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }

    [HttpPut("{id}/avatar")]
    public async Task<IActionResult> UpdateUserAvatarAsync(
        [FromRoute] int id,
        [FromForm] UpdateUserAvatarRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        if (id != userId)
        {
            return HandleFailure(
                Result.Failure(PermissionErrors.InsufficientPermissions));
        }
        
        await using var stream = request.File.OpenReadStream();
        
        var command = new UpdateUserAvatarCommand(
            userId, 
            stream,
            request.File.FileName,
            request.File.ContentType);
        
        var result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
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
    
    [HttpGet("not-connected/card/{id}")]
    public async Task<IActionResult> GetNotConnectedToCardAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetNotConnectedToCardQuery(id);
        
        Result<List<UserDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
}