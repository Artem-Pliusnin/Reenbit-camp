using Restoration.Function.Models.Enums;

namespace Restoration.Function.Models.DTos;

public class InvitationDto
{
    public int Id { get; set; }

    public int BoardId { get; set; }
    
    public int InvitedUserId { get; set; }
    
    public int? InvitedByUserId { get; set; }
    
    public InvitationStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? RespondedAt { get; set; }
}