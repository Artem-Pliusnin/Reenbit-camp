using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.BoardMembers.Commands.UpdateBoardMemberRole;

public sealed record UpdateBoardMemberRoleCommand(int BoardMemberId, BoardRole Role): ICommand;