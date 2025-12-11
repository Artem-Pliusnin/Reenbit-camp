namespace Domain.APIModels.Auth;

public sealed record LoginRequest(
    string Email,
    string Password);