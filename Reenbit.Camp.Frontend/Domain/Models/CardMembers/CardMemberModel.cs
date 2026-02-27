using Domain.Models.Users;

namespace Domain.Models.CardMembers;

public class CardMemberModel
{
    public int Id { get; set; }
    
    public UserModel User { get; set; }
}