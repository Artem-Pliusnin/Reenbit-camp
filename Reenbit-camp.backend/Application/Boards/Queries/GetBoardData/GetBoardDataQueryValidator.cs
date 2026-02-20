using FluentValidation;

namespace Application.Boards.Queries.GetBoardData;

internal class GetBoardDataQueryValidator 
    : AbstractValidator<GetBoardDataQuery>
{
    public GetBoardDataQueryValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}