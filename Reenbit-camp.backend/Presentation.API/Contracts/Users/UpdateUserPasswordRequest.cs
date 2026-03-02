namespace Presentation.API.Contracts.Users;

public sealed record UpdateUserPasswordRequest(
    string Password,
    string NewPassword
    );