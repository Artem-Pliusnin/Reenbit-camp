using System.Security.Claims;
using Application.Boards.Commands.CreateBoard;
using Application.Lists.Commands.CreateList;
using Domain.DTOs.Boards;
using Domain.DTOs.Lists;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.List;

namespace Presentation.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ListsController : ApiController
{
    public ListsController(ISender sender)
        : base(sender)
    {}
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateListRequest request, 
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
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
}