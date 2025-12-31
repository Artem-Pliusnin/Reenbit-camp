using Application.CardLabels.Commands.CreateCardLabel;
using Application.CardLabels.Commands.DeleteCardLabel;
using Application.CardLabels.Queries.GetCardLabels;
using Domain.DTOs.CardLabels;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.CardLabels;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class CardLabelsController : AuthorizedContoller
{
    public CardLabelsController(ISender sender) 
        : base(sender)
    {}
    
    [HttpGet("card/{id}")]
    public async Task<IActionResult> GetByCardAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetCardLabelsQuery(id);
        
        Result<List<CardLabelDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateCardLabelRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new CreateCardLabelCommand(request.CardId, request.LabelId);
        
        Result<CardLabelDto> result = await Sender.Send(command, cancellationToken);
        
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
        var command = new DeleteCardLabelCommand(id);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
}