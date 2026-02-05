namespace Archivation.Function.Models.Etities;

public class BoardMember
{
    public int Id { get; set; }
    
    public int BoardId { get; set; }
    
    public int UserId { get; set; }
    
    public BoardRole Role { get; set; } = BoardRole.Member;
    
    public Board Board { get; set; }
}