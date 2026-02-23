using Application.Comments.Commands.CreateComment;
using Application.Comments.Commands.DeleteComment;
using Application.Comments.Commands.UpdateCommet;
using Application.Comments.Queries.GetCardComments;
using Domain.DTOs.Comments;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Attributes;
using Presentation.API.Contracts.Comments;

namespace Presentation.API.Controllers;

[Route("api/Board/{boardId}/[controller]")]
public class CommentsController : AuthorizedContoller
{
    public CommentsController(ISender sender) 
        : base(sender)
    {}
    
    [HttpGet("card/{id}")]
    public async Task<IActionResult> GetByCardAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetCardCommentsQuery(id);
        
        Result<List<CommentDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    [RequireBoardRole(BoardRole.Viewer)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateCommentRequest request, 
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var command = new CreateCommentCommand(
            request.CardId, 
            userId, 
            request.Text);
        
        Result<CommentDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] UpdateCommentRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var command = new UpdateCommetCommand(id, userId, request.Text);
        
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
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var command = new DeleteCommentCommand(id, userId);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
}