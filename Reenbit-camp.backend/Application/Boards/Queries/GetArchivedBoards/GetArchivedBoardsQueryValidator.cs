using Domain.Models;
using FluentValidation;

namespace Application.Boards.Queries.GetArchivedBoards;

internal class GetArchivedBoardsQueryValidator 
    : AbstractValidator<GetArchivedBoardsQuery>
{
    public GetArchivedBoardsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();

        RuleFor(x => x.Filter)
            .NotEmpty()
            .SetValidator(new ArchivedBoardsFilterValidator());
    }
}

internal class ArchivedBoardsFilterValidator 
    : AbstractValidator<ArchivedBoardsFilter>
{
    public ArchivedBoardsFilterValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}