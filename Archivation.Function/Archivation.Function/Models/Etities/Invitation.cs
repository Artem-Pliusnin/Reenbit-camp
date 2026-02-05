using Archivation.Function.Models.Enums;

namespace Archivation.Function.Models.Etities;

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
}