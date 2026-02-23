using FluentValidation;

namespace Application.Labels.Queries.GetBoardLabels;

internal class GetBoardLabelsQueryValidator 
    : AbstractValidator<GetBoardLabelsQuery>
{
    public GetBoardLabelsQueryValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}