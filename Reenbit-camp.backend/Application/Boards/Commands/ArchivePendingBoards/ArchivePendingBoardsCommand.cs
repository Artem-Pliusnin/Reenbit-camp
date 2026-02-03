using Application.Abstractions.Messaging;

namespace Application.Boards.Commands.ArchivePendingBoards;

public sealed record ArchivePendingBoardsCommand() : ICommand<int>;