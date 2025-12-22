using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;

namespace Application.Boards.Queries.GetBoardData;

public sealed record GetBoardDataQuery(int BoardId) : IQuery<BoardInfoDto>;