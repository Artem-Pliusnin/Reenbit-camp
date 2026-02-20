using FluentValidation;

namespace Application.Boards.Commands.ArchiveBoard;

internal class ArchiveBoardCommandValidator 
    : AbstractValidator<ArchiveBoardCommand>
{
    public ArchiveBoardCommandValidator()
    {
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}