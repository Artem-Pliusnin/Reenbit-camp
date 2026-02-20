using FluentValidation;

namespace Application.CardMembers.Commands.DeleteCardMember;

internal class DeleteCardMemberCommandValidator 
    : AbstractValidator<DeleteCardMemberCommand>
{
    public DeleteCardMemberCommandValidator()
    {
        RuleFor(x => x.CardMemberId)
            .GreaterThan(0)
            .NotEmpty();
    }
}