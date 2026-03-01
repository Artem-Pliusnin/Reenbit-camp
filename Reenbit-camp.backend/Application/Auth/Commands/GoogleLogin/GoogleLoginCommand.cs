using Application.Abstractions.Messaging;
using Domain.DTOs.Authorization;

namespace Application.Auth.Commands.GoogleLogin;

public sealed record GoogleLoginCommand(
    string Email,
    string FirstName,
    string LastName
    ) : ICommand<TokensResponseDto>;