using Application.Abstractions.Messaging;
using Domain.DTOs.Authorization;

namespace Application.Auth.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password)
    : ICommand<TokensResponseDto>;