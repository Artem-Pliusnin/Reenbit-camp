using Domain.Enums;

namespace Domain.DTOs.BoardMembers;

public class UpdatedCardMemberRoleDto
{
    public int MemberId { get; set; }
    
    public int RoleId { get; set; }
};