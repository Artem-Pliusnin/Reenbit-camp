using Application.Abstractions.Messaging;

namespace Application.Boards.Commands.ArchiveBoard;

public sealed record ArchiveBoardCommand(int BoardId) : ICommand;