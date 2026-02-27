using Domain.Responses.Users;

namespace Domain.Responses.Comments;

public sealed record CommentDto(
    int Id, 
    UserDto User, 
    string Text, 
    bool IsEdited, 
    DateTime CreatedAt);