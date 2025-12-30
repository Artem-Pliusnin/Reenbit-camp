using Domain.Enums;
using Domain.Responses.Users;

namespace Domain.Models.BoardMembers;

public class BoardMemberModel
{
    public int Id { get; set; }
    
    public int BoardId { get; set; }
    
    public UserDto User { get; set; }
    
    public BoardRole Role { get; set; }
}