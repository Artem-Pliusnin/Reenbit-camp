using FluentValidation;

namespace Application.Comments.Commands.DeleteComment;

internal class DeleteCommentCommandValidator 
    : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.CommentId)
            .GreaterThan(0)
            .NotEmpty();
    }
}