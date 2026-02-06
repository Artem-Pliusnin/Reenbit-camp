namespace Restoration.Function.Models.Etities;

public class Label
{
    public int Id { get; set; }

    public int BoardId { get; set; }

    public string Text { get; set; }
    
    public string Color { get; set; }

    public Board Board { get; set; }
    
    public List<CardLabel> CardLabels { get; set; }
}