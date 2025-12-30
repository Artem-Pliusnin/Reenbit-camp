using Domain.DTOs.Boards;
using Domain.DTOs.Users;

namespace Domain.DTOs.Invitations;

public class InvitationDto
{
    public required int Id { get; set; }
    
    public required BoardDto Board { get; set; }
    
    public required UserDto InvitedUser { get; set; }
    
    public required UserDto InvitedByUser { get; set; }
    
    public required DateTime SentDate { get; set; }
}