using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Domain.DTOs.Authorization;
using Domain.Shared;
using MediatR;
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
        
        Result<LoginResponseDto> result =
            await Sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return Unauthorized("Invalid credentials");
        }
        
        return Ok(result.Value);
    }
}