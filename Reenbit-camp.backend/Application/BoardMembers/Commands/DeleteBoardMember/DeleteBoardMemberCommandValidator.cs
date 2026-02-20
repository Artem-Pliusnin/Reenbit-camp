using FluentValidation;

namespace Application.BoardMembers.Commands.DeleteBoardMember;

internal class DeleteBoardMemberCommandValidator 
    : AbstractValidator<DeleteBoardMemberCommand>
{
    public DeleteBoardMemberCommandValidator()
    {
        RuleFor(v => v.BoardMemberId)
            .GreaterThan(0)
            .NotEmpty();
    }
}