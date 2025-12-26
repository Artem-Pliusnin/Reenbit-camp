using Domain.Enums;

namespace Domain.Entities;

public class Invitation
{
    public int Id { get; set; }

    public int BoardId { get; set; }
    
    public int InvitedUserId { get; set; }
    
    public int? InvitedByUserId { get; set; }
    
    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? RespondedAt { get; set; }
    
    public Board Board { get; set; }
    
    public User InvitedUser { get; set; }
    
    public User? InvitedByUser { get; set; }
}