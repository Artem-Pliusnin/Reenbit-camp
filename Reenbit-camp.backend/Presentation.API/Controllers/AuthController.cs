using System.Security.Claims;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.LogoutUser;
using Application.Users.Commands.RefreshTokens;
using Application.Users.Commands.RegisterUser;
using Domain.DTOs.Authorization;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.API.Abstractions;
using Presentation.API.Contracts.Auth;

namespace Presentation.API.Controllers;

[Route("api/auth")]
public class AuthController : ApiController
{
    public AuthController(ISender sender)
        : base(sender)
    {
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);
        
        Result<int> result = await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }
        
        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);
        
        Result<TokensResponseDto> result =
            await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return Unauthorized("Invalid credentials");
        }
        
        return Ok(result.Value);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromBody] RefreshTokensRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokensCommand(
            request.RefreshToken);
        
        Result<TokensResponseDto> result = 
            await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return Unauthorized(result.Error.Message);
        }
        
        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutUser(
        [FromBody] LogoutUserRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var command = new LogoutUserCommand(
            userId, 
            request.RefreshToken);
        
        Result<bool> result = 
            await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure )
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(result.Value);
    }
}