using System.Security.Claims;
using Application.Boards.Commands.CreateBoard;
using Domain.DTOs.Boards;
using Domain.Entities;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.Boards;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class BoardsController : ApiController
{
    public BoardsController(ISender sender)
        : base(sender)
    {}

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateBoard(
        [FromBody] CreateBoardRequest request, 
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateBoardCommand(userId, request.Title);
        
        Result<BoardDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
}