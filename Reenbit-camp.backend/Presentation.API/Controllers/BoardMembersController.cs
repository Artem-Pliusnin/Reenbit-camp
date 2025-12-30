using System.Security.Claims;
using Application.BoardMembers.Commands.UpdateBoardMemberRole;
using Application.BoardMembers.Queries.GetBoardMembers;
using Application.BoardMembers.Queries.GetСurrentBoardMember;
using Domain.DTOs.BoardMembers;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.BoardMembers;

namespace Presentation.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class BoardMembersController : ApiController
{
    public BoardMembersController(ISender sender)
        : base(sender)
    {}
    
    [HttpGet("board/{id}")]
    public async Task<IActionResult> GetByBoardAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetBoardMembersQuery(id);
        
        Result<List<BoardMemberDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("board/{id}/current")]
    public async Task<IActionResult> GetCurrentAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new GetСurrentBoardMemberQuery(userId, id);
        
        Result<BoardMemberDto> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id}/role")]
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
}