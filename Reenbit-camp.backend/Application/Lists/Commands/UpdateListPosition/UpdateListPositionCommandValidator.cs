using FluentValidation;

namespace Application.Lists.Commands.UpdateListPosition;

internal class UpdateListPositionCommandValidator 
    : AbstractValidator<UpdateListPositionCommand>
{
    public UpdateListPositionCommandValidator()
    {
        RuleFor(x => x.ListId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.NewPosition)
            .GreaterThan(0)
            .NotEmpty();
    }  
}