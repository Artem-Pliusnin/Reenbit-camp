using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.FAQChat.Queries.AskQuestion;

internal class AskQuestionQueryValidator 
    : AbstractValidator<AskQuestionQuery>
{
    public AskQuestionQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Question)
            .MinimumLength(FAQValidationConstants.QuestionMinLength)
            .MaximumLength(FAQValidationConstants.QuestionMaxLength)
            .NotEmpty();
    }
}