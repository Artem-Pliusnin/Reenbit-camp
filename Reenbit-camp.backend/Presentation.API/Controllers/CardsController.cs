using System.Security.Claims;
using Application.Cards.Commands.CreateCard;
using Application.Cards.Commands.DeleteCard;
using Application.Cards.Commands.UpdateCard;
using Application.Cards.Commands.UpdateCardDeadline;
using Application.Cards.Commands.UpdateCardPosition;
using Application.Cards.Commands.UpdateCardStatus;
using Application.Cards.Queries.GetCardsByList;
using Application.Cards.Queries.GetFullCardInfo;
using Domain.DTOs.Cards;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Attributes;
using Presentation.API.Contracts.Cards;

namespace Presentation.API.Controllers;

[Route("api/Board/{boardId}/[controller]")]

public class CardsController : AuthorizedContoller
{
    public CardsController(ISender sender)
        : base(sender)
    {}
    
    [HttpGet("list/{id}")]
    public async Task<IActionResult> GetByListAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetCardsByListQuery(id);
        
        Result<List<CardDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInfoAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetFullCardInfoQuery(id);
            
        Result<CardInfoDto> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateCardRequest request, 
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new CreateCardCommand(request.ListId, request.Title, userId);
        
        Result<CardDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id}")]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] UpdateCardRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new UpdateCardCommand(
            userId, 
            id, 
            request.Title, 
            request.Description);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    [HttpPut("{id}/status")]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> UpdateStatusAsync(
        [FromBody] UpdateCardStatusRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new UpdateCardStatusCommand(
            id,
            request.IsCompleted,
            userId);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    
    [HttpPut("{id}/position")]
    public async Task<IActionResult> UpdatePositionAsync(
        [FromBody] UpdateCardPositionRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCardPositionCommand(id, request.NewListId, request.NewPosition);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    [HttpPut("{id}/deadline")]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> UpdateDeadlineAsync(
        [FromBody] UpdateCardDeadlineRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new UpdateCardDeadlineCommand(id, userId,request.StartDate,request.DueDate);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCardCommand(id);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
}