using Restoration.Function.Models.Enums;

namespace Restoration.Function.Models.DTos;

public class BoardMemberDto
{
    public int Id { get; set; }
    
    public int BoardId { get; set; }
    
    public int UserId { get; set; }
    
    public BoardRole Role { get; set; }
}