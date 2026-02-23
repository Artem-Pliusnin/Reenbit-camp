using FluentValidation;

namespace Application.Cards.Queries.GetCardsByList;

public class GetCardsByListQueryValidator 
    : AbstractValidator<GetCardsByListQuery>
{
    public GetCardsByListQueryValidator()
    {
        RuleFor(x => x.ListId)
            .GreaterThan(0)
            .NotEmpty();
    }
}