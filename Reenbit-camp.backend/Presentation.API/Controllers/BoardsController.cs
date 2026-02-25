using System.Security.Claims;
using Application.Boards.Commands.ArchiveBoard;
using Application.Boards.Commands.CreateBoard;
using Application.Boards.Commands.RestoreBoard;
using Application.Boards.Commands.UpdateBoard;
using Application.Boards.Queries.CanCreateBoard;
using Application.Boards.Queries.GetArchivedBoards;
using Application.Boards.Queries.GetBoardData;
using Application.Boards.Queries.GetUserBoards;
using Domain.Constants.HubConstants;
using Domain.DTOs.Boards;
using Domain.DTOs.Shared;
using Domain.Enums;
using Domain.Errors;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.API.Abstractions;
using Presentation.API.Attributes;
using Presentation.API.Contracts.Boards;
using Presentation.API.Hubs;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class BoardsController : AuthorizedContoller
{
    private readonly IHubContext<HomeHub> _hubContext;
    public BoardsController(
        ISender sender, 
        IHubContext<HomeHub> hubContext)
        : base(sender)
    {
        _hubContext = hubContext;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateBoardRequest request, 
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new CreateBoardCommand(userId, request.Title);
        
        Result<BoardCardDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByUserAsync(
        [FromQuery] GetBoardsQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new GetUserBoardsQuery(
            userId,
            new BoardsFilter(
                queryParameters.Title,
                queryParameters.OnlyMyBoards,
                queryParameters.Page,
                queryParameters.PageSize));
        
        Result<PaginationDto<BoardCardDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("archived")]
    public async Task<IActionResult> GetArchivedByUserAsync(
        [FromQuery] GetArchivedBoardsQueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new GetArchivedBoardsQuery(
            userId,
            new ArchivedBoardsFilter(
                queryParameters.Title,
                queryParameters.Page,
                queryParameters.PageSize));
        
        Result<PaginationDto<BoardDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("{boardId}")]
    public async Task<IActionResult> GetInfoAsync(
        [FromRoute] int boardId,
        CancellationToken cancellationToken)
    {

        var query = new GetBoardDataQuery(boardId);
            
        Result<BoardInfoDto> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{boardId}")]
    [RequireBoardRole(BoardRole.Admin)]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] UpdateBoardRequest request, 
        [FromRoute] int boardId,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new UpdateBoardCommand(userId, boardId, request.Title);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        await _hubContext.Clients.Group(HomeHub.GetBoardGroupName(boardId))
            .SendAsync(HomeHubConstants.UpdateBoardTitle, request.Title);
        
        return Ok();
    }
    
    [HttpPost("{boardId}/archive")]
    [RequireBoardRole(BoardRole.Owner)]
    public async Task<IActionResult> ArchiveBoard(
        [FromRoute] int boardId,
        CancellationToken cancellationToken)
    {
        var command = new ArchiveBoardCommand(boardId);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
    
    [HttpPost("{boardId}/restore")]
    [RequireBoardRole(BoardRole.Owner)]
    public async Task<IActionResult> RestoreBoard(
        [FromRoute] int boardId,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var command = new RestoreBoardCommand(boardId, userId);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }

    [HttpGet("can-create")]
    public async Task<IActionResult> CanCreateBoardAsync(
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var query = new CanCreateBoardQuery(userId);
        
        var result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
}