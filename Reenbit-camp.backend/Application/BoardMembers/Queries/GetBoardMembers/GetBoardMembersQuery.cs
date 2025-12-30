using Application.Abstractions.Messaging;
using Domain.DTOs.BoardMembers;

namespace Application.BoardMembers.Queries.GetBoardMembers;

public sealed record GetBoardMembersQuery(int BoardId) : IQuery<List<BoardMemberDto>>;