namespace Domain.Models.CardAttachments;

public class CardAttachmentModel
{
    public int Id { get; set; }
    
    public string FileUrl { get; set; }
    
    public string FileName { get; set; }
    
    public string ContentType { get; set; } 
}