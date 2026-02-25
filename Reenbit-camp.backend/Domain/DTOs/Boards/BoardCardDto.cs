using Domain.Enums;

namespace Domain.DTOs.Boards;

public class BoardCardDto
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public BoardStatus Status { get; set; }
    
    public int OwnerId { get; set; }
}