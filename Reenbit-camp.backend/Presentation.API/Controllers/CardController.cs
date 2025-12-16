using System.Security.Claims;
using Application.Cards.Commands.CreateCard;
using Application.Cards.Commands.DeleteCard;
using Domain.DTOs.Cards;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.Cards;

namespace Presentation.API.Controllers;

[Authorize]
[Route("api/[controller]")]

public class CardController : ApiController
{
    public CardController(ISender sender)
        : base(sender)
    {}
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateCardRequest request, 
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
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
    
    [HttpDelete("{id}")]
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