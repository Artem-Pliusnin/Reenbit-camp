using Domain.Models.Users;

namespace Domain.Models.Comments;

public class CommentModel
{
    public int Id { get; set; }

    public UserModel User { get; set; }

    public string Text { get; set; }
    
    public bool IsEdited { get; set; }
    
    public DateTime CreatedAt { get; set; }
}