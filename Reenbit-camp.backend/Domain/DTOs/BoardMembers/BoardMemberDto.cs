using Domain.DTOs.Users;
using Domain.Enums;

namespace Domain.DTOs.BoardMembers;

public class BoardMemberDto
{
    public int Id { get; set; }
    
    public int BoardId { get; set; }
    
    public UserDto User { get; set; }
    
    public BoardRole Role { get; set; } = BoardRole.Member;
}