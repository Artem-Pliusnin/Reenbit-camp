namespace Restoration.Function.Models.Etities;

public class CardAttachment
{
    public int Id { get; set; }

    public int CardId { get; set; }

    public string FileUrl { get; set; }
    
    public string FileName { get; set; }
    
    public string ContentType { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Card Card { get; set; }
}