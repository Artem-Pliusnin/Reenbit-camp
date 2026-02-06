namespace Restoration.Function.Models.DTos;

public class CommentDto
{
    public int Id { get; set; }

    public int CardId { get; set; }
    
    public int UserId { get; set; }
    
    public string Text { get; set; }
    
    public bool IsEdited { get; set; }
    
    public DateTime CreatedAt { get; set; }
}