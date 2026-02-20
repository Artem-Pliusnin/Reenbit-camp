using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.CardMembers.Commands.CreateCardMember;

internal class CreateCardMemberCommandValidator 
    : AbstractValidator<CreateCardMemberCommand>
{
    public CreateCardMemberCommandValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .NotEmpty();
    }
}