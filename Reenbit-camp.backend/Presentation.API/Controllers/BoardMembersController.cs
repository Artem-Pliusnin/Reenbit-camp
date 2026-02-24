using System.Security.Claims;
using Application.BoardMembers.Commands.DeleteBoardMember;
using Application.BoardMembers.Commands.UpdateBoardMemberRole;
using Application.BoardMembers.Queries.GetBoardMembers;
using Application.BoardMembers.Queries.GetСurrentBoardMember;
using Domain.DTOs.BoardMembers;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Attributes;
using Presentation.API.Contracts.BoardMembers;

namespace Presentation.API.Controllers;

[Route("api/Board/{boardId}/[controller]")]
public class BoardMembersController : AuthorizedContoller
{
    public BoardMembersController(ISender sender)
        : base(sender)
    {}
    
    [HttpGet]
    public async Task<IActionResult> GetByBoardAsync(
        [FromRoute] int boardId,
        CancellationToken cancellationToken)
    {
        var query = new GetBoardMembersQuery(boardId);
        
        Result<List<BoardMemberDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentAsync(
        [FromRoute] int boardId,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new GetСurrentBoardMemberQuery(userId, boardId);
        
        Result<BoardMemberDto> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id}/role")]
    [RequireBoardRole(BoardRole.Admin)]
    public async Task<IActionResult> UpdateRoleAsync(
        [FromBody] UpdateBoardMemberRoleRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new UpdateBoardMemberRoleCommand(id, request.Role);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBoardMemberCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
}