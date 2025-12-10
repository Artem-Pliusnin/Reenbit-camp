namespace Reenbit.Camp.Frontend.Models.Auth.APIModels;

public sealed record LoginRequest(
    string Email,
    string Password);