using Application.Abstractions.Messaging;

namespace Application.Boards.Queries.CanCreateBoard;

public sealed record CanCreateBoardQuery(int UserId) : IQuery<bool>;