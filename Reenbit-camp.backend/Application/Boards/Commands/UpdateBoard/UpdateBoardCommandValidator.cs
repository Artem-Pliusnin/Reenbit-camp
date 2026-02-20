using Domain.Constants.ValidationConstants;
using FluentValidation;

namespace Application.Boards.Commands.UpdateBoard;

public class UpdateBoardCommandValidator : AbstractValidator<UpdateBoardCommand>
{
    public UpdateBoardCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.BoardId)
            .GreaterThan(0)
            .NotEmpty();

        RuleFor(x => x.Title)
            .MaximumLength(BoardValidationConstants.TitleMaxLength)
            .MinimumLength(BoardValidationConstants.TitleMinLength)
            .NotEmpty();
    }
}