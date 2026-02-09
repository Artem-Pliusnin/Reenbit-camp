using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;
using Domain.DTOs.Shared;
using Domain.Models;

namespace Application.Boards.Queries.GetArchivedBoards;

public sealed record GetArchivedBoardsQuery(
    int UserId, 
    ArchivedBoardsFilter Filter) 
    : IQuery<PaginationDto<BoardDto>>;