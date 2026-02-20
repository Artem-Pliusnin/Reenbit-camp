using FluentValidation;

namespace Application.CardLabels.Queries.GetCardLabels;

internal class GetCardLabelsQueryValidator 
    : AbstractValidator<GetCardLabelsQuery>
{
    public GetCardLabelsQueryValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}