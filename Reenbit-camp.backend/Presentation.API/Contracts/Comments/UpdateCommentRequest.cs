namespace Presentation.API.Contracts.Comments;

public sealed record UpdateCommentRequest(
    int CommentId, 
    string Text);