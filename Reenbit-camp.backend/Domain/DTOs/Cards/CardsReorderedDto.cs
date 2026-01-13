namespace Domain.DTOs.Cards;

public class CardsReorderedDto
{
    public int BoardId { get; set; }
    
    public int SourceListId { get; set; }
    
    public int DestinationListId { get; set; }

    public List<int> OrderedCardIds { get; set; }
}