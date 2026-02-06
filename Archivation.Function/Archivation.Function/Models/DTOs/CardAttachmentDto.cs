namespace Archivation.Function.Models.DTos;

public class CardAttachmentDto
{
    public int Id { get; set; }

    public int CardId { get; set; }

    public string FileUrl { get; set; }
    
    public string FileName { get; set; }
    
    public string ContentType { get; set; }
    
    public DateTime CreatedAt { get; set; }
}