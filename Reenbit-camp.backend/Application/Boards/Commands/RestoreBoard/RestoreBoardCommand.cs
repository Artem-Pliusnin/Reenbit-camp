using Application.Abstractions.Messaging;

namespace Application.Boards.Commands.RestoreBoard;

public sealed record RestoreBoardCommand(
    int BoardId,
    int UserId) : ICommand;