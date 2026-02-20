using FluentValidation;

namespace Application.FAQChat.Commands.StartChatSession;

internal class StartChatSessionCommandValidator 
    : AbstractValidator<StartChatSessionCommand>
{
    public StartChatSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}