namespace Domain.DTOs.Cards;

public class CardInfoDto
{
    public required int Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public required bool IsCompleted { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? DueDate { get; set; }
}