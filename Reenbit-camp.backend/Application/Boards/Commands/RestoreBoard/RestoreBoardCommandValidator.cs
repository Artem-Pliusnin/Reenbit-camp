using FluentValidation;

namespace Application.Boards.Commands.RestoreBoard;

public class RestoreBoardCommandValidator 
    : AbstractValidator<RestoreBoardCommand>
{
    public RestoreBoardCommandValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}