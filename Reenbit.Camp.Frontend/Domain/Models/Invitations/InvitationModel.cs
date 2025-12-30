using Domain.Models.Boards;
using Domain.Models.Users;

namespace Domain.Models.Invitations;

public class InvitationModel
{
    public int Id { get; set; }
    
    public BoardModel Board { get; set; }
    
    public UserModel InvitedUser { get; set; }
    
    public UserModel InvitedByUser { get; set; }
    
    public DateTime SentDate { get; set; }
}