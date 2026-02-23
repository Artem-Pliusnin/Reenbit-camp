using Application.Invitations.Commands.AcceptInvitation;
using Application.Invitations.Commands.CreateInvitation;
using Application.Invitations.Commands.DeclinedInvitation;
using Application.Invitations.Commands.DeleteInvitation;
using Application.Invitations.Queries.GetInvitationsByBoard;
using Application.Invitations.Queries.GetUserInvitations;
using Domain.Constants.HubConstants;
using Domain.DTOs.Invitations;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.API.Abstractions;
using Presentation.API.Attributes;
using Presentation.API.Contracts.Invitations;
using Presentation.API.Hubs;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class InvitationsController : AuthorizedContoller
{
    private readonly IHubContext<HomeHub> _hubContext;
    public InvitationsController(ISender sender, IHubContext<HomeHub> hubContext)
        : base(sender)
    {
        _hubContext = hubContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByUserAsync(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new GetUserInvitationsQuery(userId);
        
        Result<List<InvitationDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetByBoardAsync(
        [FromRoute] int boardId, 
        CancellationToken cancellationToken)
    {
        var query = new GetInvitationsByBoardQuery(boardId);
        
        Result<List<InvitationDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost("board/{boardId}")]
    [RequireBoardRole(BoardRole.Admin)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateInvitationRequest request, 
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new CreateInvitationCommand(
            request.BoardId, 
            request.InvitedUserId, 
            userId);
        
        Result<InvitationDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        await _hubContext.Clients
            .User(result.Value.InvitedUser.Id.ToString())
            .SendAsync(
                HomeHubConstants.AddUserInvitation, 
                result.Value, 
                cancellationToken);
            
        return Ok(result.Value);
    }
    
    [HttpPut("{id}/accept")]
    public async Task<IActionResult> AcceptInvitationAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new AcceptInvitationCommand(userId, id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        await _hubContext.Clients
            .Group(HomeHub.GetBoardGroupName(result.Value.BoardId))
            .SendAsync(HomeHubConstants.AddMember, result.Value, cancellationToken);
        
        await _hubContext.Clients
            .Group(HomeHub.GetBoardGroupName(result.Value.BoardId))
            .SendAsync(HomeHubConstants.DeleteInvitation, id, cancellationToken);
        
        return Ok();
    }
    
    [HttpPut("{id}/decline")]
    public async Task<IActionResult> DeclineInvitationAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new DeclineInvitationCommand(userId, id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        await _hubContext.Clients
            .Group(HomeHub.GetBoardGroupName(result.Value.Board.Id))
            .SendAsync(HomeHubConstants.DeleteInvitation, result.Value.Id, cancellationToken);
        
        return Ok();
    }
    
    [HttpDelete("board/{boardId}/invitation/{id}")]
    [RequireBoardRole(BoardRole.Admin)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteInvitationCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        await _hubContext.Clients
            .User(result.Value.InvitedUser.Id.ToString())
            .SendAsync(HomeHubConstants.DeleteUserInvitation, id, cancellationToken);
        
        await _hubContext.Clients
            .Group(HomeHub.GetBoardGroupName(result.Value.Board.Id))
            .SendAsync(HomeHubConstants.DeleteInvitation, result.Value.Id, cancellationToken);
        
        return Ok();
    }
}