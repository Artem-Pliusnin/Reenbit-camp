using Archivation.Function.Models.Enums;

namespace Archivation.Function.Models.DTos;

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