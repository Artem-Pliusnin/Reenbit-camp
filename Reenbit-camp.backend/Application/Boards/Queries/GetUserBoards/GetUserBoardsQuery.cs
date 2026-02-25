using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;
using Domain.DTOs.Shared;
using Domain.Models;

namespace Application.Boards.Queries.GetUserBoards;

public sealed record GetUserBoardsQuery(int UserId, BoardsFilter Filter) : IQuery<PaginationDto<BoardCardDto>>;