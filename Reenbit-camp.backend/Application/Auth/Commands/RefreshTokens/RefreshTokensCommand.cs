using Application.Abstractions.Messaging;
using Domain.DTOs.Authorization;

namespace Application.Auth.Commands.RefreshTokens;

public sealed record RefreshTokensCommand(
    string RefreshToken)
    : ICommand<TokensResponseDto>;
