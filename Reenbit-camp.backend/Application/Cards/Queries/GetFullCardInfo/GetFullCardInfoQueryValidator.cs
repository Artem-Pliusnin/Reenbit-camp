using FluentValidation;

namespace Application.Cards.Queries.GetFullCardInfo;

public class GetFullCardInfoQueryValidator 
    : AbstractValidator<GetFullCardInfoQuery>
{
    public GetFullCardInfoQueryValidator()
    {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .NotEmpty();
    }
}