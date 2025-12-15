using Application.Abstractions.Messaging;
using Domain.DTOs.Lists;

namespace Application.Lists.Queries;

public sealed record GetListsByBoardQuery(int BoardId) : IQuery<List<ListDto>>;