using System.Security.Claims;
using Application.Lists.Commands.CreateList;
using Application.Lists.Commands.DeleteList;
using Application.Lists.Commands.UpdateList;
using Application.Lists.Commands.UpdateListPosition;
using Application.Lists.Queries.GetListsByBoard;
using Domain.DTOs.Lists;
using Domain.Enums;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.API.Abstractions;
using Presentation.API.Attributes;
using Presentation.API.Contracts.List;
using Presentation.API.Hubs;

namespace Presentation.API.Controllers;

[Route("api/Board/{boardId}/[controller]")]
public class ListsController : AuthorizedContoller
{
    private readonly IHubContext<HomeHub> _hubContext;
    public ListsController(ISender sender, IHubContext<HomeHub> hubContext)
        : base(sender)
    {
        _hubContext = hubContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByBoardAsync(
        [FromRoute] int boardId,
        CancellationToken cancellationToken)
    {
        var query = new GetListsByBoardQuery(boardId);
        
        Result<List<ListDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateListRequest request, 
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new CreateListCommand(request.BoardId, request.Title, userId);
        
        Result<ListDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] UpdateListRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new UpdateListCommand(userId, id, request.Title);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    [HttpPut("{id}/position")]
    [RequireBoardRole(BoardRole.Member)]
    public async Task<IActionResult> UpdatePositionAsync(
        [FromBody] UpdateListPositionRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new UpdateListPositionCommand(id, request.NewPosition, userId);
        
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
        var command = new DeleteListCommand(id);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
}