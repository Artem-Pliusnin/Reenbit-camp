using System.Security.Claims;
using Application.Auth.Commands.LoginUser;
using Application.Auth.Commands.LogoutUser;
using Application.Auth.Commands.RefreshTokens;
using Application.Auth.Commands.RegisterUser;
using Domain.DTOs.Authorization;
using Domain.Errors;
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
            return HandleUnauthorized(result);
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
            return  HandleUnauthorized(result);
        }
        
        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutUser(
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out var userId))
        {
            return  HandleUnauthorized(
                Result.Failure(UserErrors.UserUnauthorized));
        }

        var command = new LogoutUserCommand(userId);
        
        Result<bool> result = 
            await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure )
        {
            return HandleUnauthorized(result);
        }
        
        return Ok(result.Value);
    }
}