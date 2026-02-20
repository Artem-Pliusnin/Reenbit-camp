using Domain.Models;
using FluentValidation;

namespace Application.Boards.Queries.GetUserBoards;

internal class GetUserBoardsQueryValidator 
    : AbstractValidator<GetUserBoardsQuery>
{
    public GetUserBoardsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();

        RuleFor(x => x.Filter)
            .NotEmpty()
            .SetValidator(new BoardsFilterValidator());
    }
}

internal class BoardsFilterValidator 
    : AbstractValidator<BoardsFilter>
{
    public BoardsFilterValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}