using Domain.Shared;

namespace Domain.Errors;

public static class CommentErrors
{
    public static readonly Error CreateCommentError = new(
        "Comment.CreateError",
        "Error occured while creating the comment.");
    
    public static readonly Error CommentDoesNotExist = new(
        "Comment.DoesNotExist",
        "Comment with given id does not exist.");
    
    public static readonly Error UpdateCommentError = new(
        "Comment.UpdateError",
        "Error occured while updating the comment.");
    
    public static readonly Error CommentDoesNotBelongsToUserError = new(
        "Comment.DoesNotBelongsToUserError",
        "Comment does not belong to this user.");
}