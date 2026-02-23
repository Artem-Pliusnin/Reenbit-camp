using FluentValidation;

namespace Application.CardMembers.Queries.GetNotConnectedToCard;

public class GetNotConnectedToCardQueryValidator 
    : AbstractValidator<GetNotConnectedToCardQuery>
{
    public GetNotConnectedToCardQueryValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}