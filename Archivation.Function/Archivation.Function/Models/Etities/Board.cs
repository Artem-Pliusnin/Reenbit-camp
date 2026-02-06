using Archivation.Function.Models.Enums;

namespace Archivation.Function.Models.Etities;

public class Board
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public int CreatedBy { get; set; } = 1;
    
    public DateTime CreationDate { get; set; }
    
    public int? LastUpdatedBy { get; set; }
    
    public DateTime? LastUpdateDate { get; set; }
    
    public BoardStatus Status { get; set; } = BoardStatus.Active;
    
    public List<Invitation> Invitations { get; set; }
    
    public List<BoardMember> Members { get; set; }
    
    public List<List> Lists { get; set; }
    
    public List<Label> Labels { get; set; }
}