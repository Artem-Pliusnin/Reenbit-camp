using Domain.DTOs.Users;

namespace Domain.DTOs.CardMembers;

public class CardMemberDto
{
    public required int Id { get; set; }
    
    public required UserDto User { get; set; }
}