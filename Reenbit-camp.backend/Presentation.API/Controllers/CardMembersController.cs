using Application.CardMembers.Commands.CreateCardMember;
using Application.CardMembers.Commands.DeleteCardMember;
using Application.CardMembers.Queries.GetCardMembers;
using Domain.DTOs.CardMembers;
using Domain.Enums;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Attributes;
using Presentation.API.Contracts.CardMembers;

namespace Presentation.API.Controllers;

[Route("api/Board/{boardId}/[controller]")]
public class CardMembersController : AuthorizedContoller
{
    public CardMembersController(ISender sender) 
        : base(sender)
    {}
    
    [HttpGet("card/{id}")]
    public async Task<IActionResult> GetByCardAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetCardMembersQuery(id);
        
        Result<List<CardMemberDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateCardMemberRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new CreateCardMemberCommand(request.CardId, request.UserId);
        
        Result<CardMemberDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpDelete("{id}")]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCardMemberCommand(id);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
}