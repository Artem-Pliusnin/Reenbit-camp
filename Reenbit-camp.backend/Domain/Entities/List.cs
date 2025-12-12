namespace Domain.Entities;

public class List
{
    public int Id { get; set; }

    public int BoardId { get; set; } = 1;
    
    public string Title { get; set; }
    
    public int Position { get; set; }
    
    public int? LastUpdatedBy { get; set; }
    
    public DateTime? LastUpdateDate { get; set; }
    
    public Board Board { get; set; }
    
    public User? LastUpdatedByUser { get; set; }
}