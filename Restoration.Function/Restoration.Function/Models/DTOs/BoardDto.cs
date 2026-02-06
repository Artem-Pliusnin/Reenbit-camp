using Restoration.Function.Models.Enums;

namespace Restoration.Function.Models.DTos;

public class BoardDto
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public int CreatedBy { get; set; } = 1;
    
    public DateTime CreationDate { get; set; }
    
    public int? LastUpdatedBy { get; set; }
    
    public DateTime? LastUpdateDate { get; set; }
    
    public BoardStatus Status { get; set; }
    
    public List<InvitationDto> Invitations { get; set; }
    
    public List<BoardMemberDto> Members { get; set; }
    
    public List<ListDto> Lists { get; set; }
    
    public List<LabelDto> Labels { get; set; }
}