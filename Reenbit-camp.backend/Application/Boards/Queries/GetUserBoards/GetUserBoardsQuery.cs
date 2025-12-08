using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;

namespace Application.Boards.Queries.GetUserBoards;

public sealed record GetUserBoardsQuery(int UserId) : IQuery<List<BoardDto>>;