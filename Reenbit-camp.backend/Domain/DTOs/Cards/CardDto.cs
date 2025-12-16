namespace Domain.DTOs.Cards;

public class CardDto
{
    public required int Id { get; set; }
    
    public required string Title { get; set; }
    
    public required int Position { get; set; }
    
    public required DateTime? StartDate { get; set; }
    
    public required DateTime? DueDate { get; set; }
}