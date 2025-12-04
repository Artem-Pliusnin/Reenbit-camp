namespace Presentation.API.Contracts.Auth;

public sealed record RegisterUserRequest(string FirstName,
    string LastName,
    string Email,
    string Password);