namespace Domain.DTOs.CardAttachments;

public class CardAttachmentDto
{
    public required int Id { get; set; }
    
    public required string FileUrl { get; set; }
    
    public required string FileName { get; set; }
    
    public required string ContentType { get; set; } 
}