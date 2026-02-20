using FluentValidation;

namespace Application.Labels.Queries.GetLabelsForCard;

internal class GetLabelsForCardQueryValidator 
    : AbstractValidator<GetLabelsForCardQuery>
{
    public GetLabelsForCardQueryValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}