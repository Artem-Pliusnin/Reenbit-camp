using Archivation.Function.Models.Etities;

namespace Archivation.Function.Models.DTos;

public class BoardMemberDto
{
    public int Id { get; set; }
    
    public int BoardId { get; set; }
    
    public int UserId { get; set; }
    
    public BoardRole Role { get; set; }
}