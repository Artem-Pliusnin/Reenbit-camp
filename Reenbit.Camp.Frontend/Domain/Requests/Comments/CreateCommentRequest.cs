namespace Domain.Requests.Comments;

public sealed record CreateCommentRequest(
    int CardId, 
    string Text);