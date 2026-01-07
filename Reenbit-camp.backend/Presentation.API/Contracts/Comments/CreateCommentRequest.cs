namespace Presentation.API.Contracts.Comments;

public sealed record CreateCommentRequest(
    int CardId, 
    string Text);