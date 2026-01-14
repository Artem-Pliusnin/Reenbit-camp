namespace Domain.Models.Cards;

public class CardsReorderedModel
{
    public int BoardId { get; set; }
    
    public int SourceListId { get; set; }
    
    public int DestinationListId { get; set; }

    public List<int> OrderedCardIds { get; set; }
}