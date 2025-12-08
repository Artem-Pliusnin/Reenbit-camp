using Application.Abstractions.Messaging;

namespace Application.Boards.Commands.UpdateBoard;

public sealed record UpdateBoardCommand(
    int UserId,
    int BoardId,
    string Title) : ICommand;