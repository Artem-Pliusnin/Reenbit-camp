using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Boards.Commands.CreateBoard;

internal class CreateBoardCommandValidator 
    : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();

        RuleFor(x => x.Title)
            .MaximumLength(BoardValidationConstants.TitleMaxLength)
            .MinimumLength(BoardValidationConstants.TitleMinLength)
            .NotEmpty();
    }
}