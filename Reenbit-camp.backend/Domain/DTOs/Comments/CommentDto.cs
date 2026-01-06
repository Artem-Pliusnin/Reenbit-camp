using Domain.DTOs.Users;

namespace Domain.DTOs.Comments;

public class CommentDto
{
    public required int Id { get; set; }

    public required UserDto User { get; set; }

    public required string Text { get; set; }
    
    public required bool IsEdited { get; set; }
    
    public required DateTime CreatedAt { get; set; }
}