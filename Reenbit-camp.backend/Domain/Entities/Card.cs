namespace Domain.Entities;

public class Card
{
    public int Id { get; set; }

    public int ListId { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public int Position { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public int? LastUpdatedBy { get; set; }
    
    public DateTime? LastUpdateDate { get; set; }
    
    public List List { get; set; }
    
    public User? LastUpdatedByUser { get; set; }
    
    public List<CardLabel> Labels { get; set; }
    
    public List<CardMember> Members { get; set; }
    
    public List<CardAttachment> Attachments { get; set; }
    
    public List<Comment> Comments { get; set; }
}