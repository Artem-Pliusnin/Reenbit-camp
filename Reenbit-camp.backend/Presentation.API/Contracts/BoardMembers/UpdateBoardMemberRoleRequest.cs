using Domain.Enums;

namespace Presentation.API.Contracts.BoardMembers;

public sealed record UpdateBoardMemberRoleRequest(int BoardMemberId, BoardRole Role);