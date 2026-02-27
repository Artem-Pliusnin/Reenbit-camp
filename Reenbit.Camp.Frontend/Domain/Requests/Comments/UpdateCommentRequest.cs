namespace Domain.Requests.Comments;

public sealed record UpdateCommentRequest(
    int CommentId, 
    string Text);