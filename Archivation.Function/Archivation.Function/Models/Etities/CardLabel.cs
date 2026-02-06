namespace Archivation.Function.Models.Etities;

public class CardLabel
{
    public int Id { get; set; }

    public int CardId { get; set; }

    public int LabelId { get; set; }
    
    public Card Card { get; set; }
    
    public Label Label { get; set; }
}