using Domain.Enums;

namespace Domain.Models.Boards;

public class BoardCardModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public BoardStatus Status { get; set; }
    
    public int OwnerId { get; set; }
}