using System.Security.Claims;
using Application.Boards.Commands.CreateBoard;
using Application.Boards.Commands.UpdateBoard;
using Application.Boards.Queries.GetBoardData;
using Application.Boards.Queries.GetUserBoards;
using Domain.DTOs.Boards;
using Domain.DTOs.Shared;
using Domain.Errors;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.Boards;

namespace Presentation.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class BoardsController : ApiController
{
    public BoardsController(ISender sender)
        : base(sender)
    {}
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateBoardRequest request, 
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new CreateBoardCommand(userId, request.Title);
        
        Result<BoardDto> result = await Sender.Send(command, cancellationToken);
        
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
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
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
        
        Result<PaginationDto<BoardDto>> result = await Sender.Send(query, cancellationToken);
        
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

        var query = new GetBoardDataQuery(id);
            
        Result<BoardInfoDto> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] UpdateBoardRequest request, 
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new UpdateBoardCommand(userId, id, request.Title);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }

}