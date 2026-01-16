using Application.CardAttachments.Commands.CreateCardAttachment;
using Application.CardAttachments.Commands.DeleteCardAttachment;
using Application.CardAttachments.Queries.GetCardAttachments;
using Domain.DTOs.CardAttachments;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.CardAttachments;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class CardAttachmentsController : ApiController
{
    public CardAttachmentsController(ISender sender) 
        : base(sender)
    {}
    
    [HttpGet("card/{id}")]
    public async Task<IActionResult> GetByCardAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetCardAttachmentsQuery(id);
        
        Result<List<CardAttachmentDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromForm] CreateCardAttachmentRequest request,
        CancellationToken cancellationToken)
    {
        await using var stream = request.file.OpenReadStream();
        
        var command = new CreateCardAttachmentCommand(
            request.cardId, 
            stream,
            request.file.FileName,
            request.file.ContentType);
        
        Result<CardAttachmentDto> result = await Sender.Send(command, cancellationToken);
        
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
        var command = new DeleteCardAttachmentCommand(id);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
}