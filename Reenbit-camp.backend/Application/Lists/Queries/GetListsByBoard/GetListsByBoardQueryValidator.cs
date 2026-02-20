using FluentValidation;

namespace Application.Lists.Queries.GetListsByBoard;

internal class GetListsByBoardQueryValidator 
    : AbstractValidator<GetListsByBoardQuery>
{
    public GetListsByBoardQueryValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}