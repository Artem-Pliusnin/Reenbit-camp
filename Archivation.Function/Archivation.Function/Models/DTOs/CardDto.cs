namespace Archivation.Function.Models.DTos;

public class CardDto
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
    
    public List<CardLabelDto> Labels { get; set; }
    
    public List<CardMemberDto> Members { get; set; }
    
    public List<CardAttachmentDto> Attachments { get; set; }
    
    public List<CommentDto> Comments { get; set; }
}