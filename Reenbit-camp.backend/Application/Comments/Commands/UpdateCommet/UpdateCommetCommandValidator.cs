using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Comments.Commands.UpdateCommet;

public class UpdateCommetCommandValidator 
    : AbstractValidator<UpdateCommetCommand>
{
    public UpdateCommetCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.CommentId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Text)
            .MinimumLength(CommentValidationConstants.CommentMinLength)
            .MaximumLength(CommentValidationConstants.CommentMinLength)
            .NotEmpty();
    }
}