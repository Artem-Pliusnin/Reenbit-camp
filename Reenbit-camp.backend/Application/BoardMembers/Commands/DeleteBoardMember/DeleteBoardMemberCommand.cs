using Application.Abstractions.Messaging;

namespace Application.BoardMembers.Commands.DeleteBoardMember;

public sealed record DeleteBoardMemberCommand(int BoardMemberId) : ICommand;