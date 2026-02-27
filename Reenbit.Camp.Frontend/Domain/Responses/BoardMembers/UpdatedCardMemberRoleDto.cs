using Domain.Enums;

namespace Domain.Responses.BoardMembers;

public sealed record UpdatedCardMemberRoleDto(
    int MemberId,
    int RoleId);