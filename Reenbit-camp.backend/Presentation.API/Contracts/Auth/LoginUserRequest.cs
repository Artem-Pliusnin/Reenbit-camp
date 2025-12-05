namespace Presentation.API.Contracts.Auth;

public sealed record LoginUserRequest(
    string Email,
    string Password);