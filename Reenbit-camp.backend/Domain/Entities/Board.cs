namespace Domain.Entities;

public class Board
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public int CreatedBy { get; set; } = 1;
    
    public DateTime CreationDate { get; set; }
    
    public int? LastUpdatedBy { get; set; }
    
    public DateTime? LastUpdateDate { get; set; }
    
    public User CreatedByUser { get; set; }
    public User? LastUpdatedByUser { get; set; }
}