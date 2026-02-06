namespace Restoration.Function.Models.DTos;

public class ListDto
{
    public int Id { get; set; }

    public int BoardId { get; set; }
    
    public string Title { get; set; }
    
    public int Position { get; set; }
    
    public int? LastUpdatedBy { get; set; }
    
    public DateTime? LastUpdateDate { get; set; }
    
    public List<CardDto> Cards { get; set; }
}