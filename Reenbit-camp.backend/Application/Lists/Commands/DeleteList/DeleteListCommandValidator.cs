using FluentValidation;

namespace Application.Lists.Commands.DeleteList;

internal class DeleteListCommandValidator 
    : AbstractValidator<DeleteListCommand>
{
    public DeleteListCommandValidator()
    {
        RuleFor(x => x.ListId)
            .GreaterThan(0)
            .NotEmpty();
    }
}