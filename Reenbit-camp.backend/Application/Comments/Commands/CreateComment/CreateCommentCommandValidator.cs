using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Comments.Commands.CreateComment;

internal class CreateCommentCommandValidator 
    : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Text)
            .MinimumLength(CommentValidationConstants.CommentMinLength)
            .MaximumLength(CommentValidationConstants.CommentMinLength)
            .NotEmpty();
    }
}