using Application.Abstractions.Messaging;
using Domain.DTOs.BoardMembers;

namespace Application.BoardMembers.Queries.GetСurrentBoardMember;

public sealed record GetСurrentBoardMemberQuery(
    int UserId, 
    int BoardId)
    : IQuery<BoardMemberDto>;