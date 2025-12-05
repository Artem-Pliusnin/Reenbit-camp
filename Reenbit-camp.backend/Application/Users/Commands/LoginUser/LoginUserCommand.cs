using Application.Abstractions.Messaging;
using Domain.DTOs.Authorization;

namespace Application.Users.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password)
    : ICommand<LoginResponseDto>;