namespace Domain.Models.Cards;

public class CardModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int Position { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? DueDate { get; set; }
}