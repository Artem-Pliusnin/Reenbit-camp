using FluentValidation;

namespace Application.FAQChat.Commands.EndChatSession;

internal class EndChatSessionCommandValidator 
    : AbstractValidator<EndChatSessionCommand>
{
    public EndChatSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}