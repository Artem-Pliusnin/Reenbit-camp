using Application.Labels.Commands.CreateLabel;
using Application.Labels.Commands.DeleteLabel;
using Application.Labels.Commands.UpdateLabel;
using Application.Labels.Queries.GetBoardLabels;
using Application.Labels.Queries.GetLabelsForCard;
using Domain.DTOs.Labels;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.Labels;
using Presentation.API.Hubs;

namespace Presentation.API.Controllers;

[Route("api/[controller]")]
public class LabelsController : AuthorizedContoller
{
    private readonly IHubContext<HomeHub> _hubContext;
    public LabelsController(ISender sender, IHubContext<HomeHub> hubContext) 
        : base(sender)
    {
        _hubContext = hubContext;
    }
    
    [HttpGet("board/{id}")]
    public async Task<IActionResult> GetByBoardAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetBoardLabelsQuery(id);
        
        Result<List<LabelDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet("not-connected/card/{id}")]
    public async Task<IActionResult> GetNotConnectedAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetLabelsForCardQuery(id);
        
        Result<List<LabelDto>> result = await Sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateLabelRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new CreateLabelCommand(
            request.BoardId, 
            request.Text, 
            request.Color);
        
        Result<LabelDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [FromBody] UpdateLabelRequest request,
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }
        
        var command = new UpdateLabelCommand(id, request.Text, request.Color);
        
        Result<UpdatedLabelDto> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        await _hubContext.Clients
            .Group(HomeHub.GetBoardGroupName(result.Value.BoardId))
            .SendAsync("UpdateLabel", result.Value.Label, cancellationToken);
        
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteLabelCommand(id);
        
        Result result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok();
    }
}