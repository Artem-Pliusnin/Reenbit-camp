using FluentValidation;

namespace Application.Comments.Queries.GetCardComments;

public class GetCardCommentsQueryValidator 
    : AbstractValidator<GetCardCommentsQuery>
{
    public GetCardCommentsQueryValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}