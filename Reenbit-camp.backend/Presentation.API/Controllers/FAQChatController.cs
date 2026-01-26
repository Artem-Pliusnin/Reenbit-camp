using Application.FAQChat.Commands.EndChatSession;
using Application.FAQChat.Commands.ImportFile;
using Application.FAQChat.Commands.StartChatSession;
using Application.FAQChat.Queries.AskQuestion;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.FAQ;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class FAQChatController : AuthorizedContoller
{
    public FAQChatController(ISender sender) : 
        base(sender)
    {}
    
    [HttpPost("question")]
    public async Task<IActionResult> AskQuestionAsync(
        [FromBody] AskQuestionRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new AskQuestionQuery(userId, request.Question);
        
        Result<string> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost("file/import")]
    public async Task<IActionResult> ImportFileAsync(
        [FromForm] ImportFileRequest request, 
        CancellationToken cancellationToken)
    {
        await using var stream = request.File.OpenReadStream();
        
        var command = new ImportFileCommand(
            stream,
            request.File.FileName);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    [HttpPost("session/start")]
    public async Task<IActionResult> StartSessionAsync(
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new StartChatSessionCommand(userId);
        
        Result result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    [HttpPost("session/end")]
    public async Task<IActionResult> EndSessionAsync(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new EndChatSessionCommand(userId);
        
        Result result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok();
    }
}