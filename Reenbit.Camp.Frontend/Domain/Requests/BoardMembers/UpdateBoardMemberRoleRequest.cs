using Domain.Enums;

namespace Domain.Requests.BoardMembers;

public sealed record UpdateBoardMemberRoleRequest(int BoardMemberId, BoardRole Role);