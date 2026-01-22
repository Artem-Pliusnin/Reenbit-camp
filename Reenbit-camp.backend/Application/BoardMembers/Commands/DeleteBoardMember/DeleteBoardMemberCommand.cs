using Application.Abstractions.Messaging;
using Domain.DTOs.BoardMembers;

namespace Application.BoardMembers.Commands.DeleteBoardMember;

public sealed record DeleteBoardMemberCommand(int BoardMemberId) : ICommand<DeleteBoardMemberDto>;