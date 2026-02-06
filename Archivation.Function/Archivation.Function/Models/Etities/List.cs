namespace Archivation.Function.Models.Etities;

public class List
{
    public int Id { get; set; }

    public int BoardId { get; set; }
    
    public string Title { get; set; }
    
    public int Position { get; set; }
    
    public int? LastUpdatedBy { get; set; }
    
    public DateTime? LastUpdateDate { get; set; }
    
    public Board Board { get; set; }
    
    public List<Card> Cards { get; set; }
}