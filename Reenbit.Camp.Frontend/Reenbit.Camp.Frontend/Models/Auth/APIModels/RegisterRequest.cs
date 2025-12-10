namespace Reenbit.Camp.Frontend.Models.Auth.APIModels;

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);