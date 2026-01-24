using Application.Comments.Commands.CreateComment;
using Application.FAQChat.Commands.ImportFile;
using Application.FAQChat.Queries.AskQuestion;
using Domain.DTOs.Comments;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.Comments;
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
        [FromBody] string question,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new AskQuestionQuery(userId, question);
        
        Result<string> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
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
}