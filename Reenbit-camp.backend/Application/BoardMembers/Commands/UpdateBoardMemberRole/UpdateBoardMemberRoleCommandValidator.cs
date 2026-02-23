using FluentValidation;

namespace Application.BoardMembers.Commands.UpdateBoardMemberRole;

internal class UpdateBoardMemberRoleCommandValidator 
    : AbstractValidator<UpdateBoardMemberRoleCommand>
{
    public UpdateBoardMemberRoleCommandValidator()
    {
        RuleFor(x => x.BoardMemberId)
            .GreaterThan(0)
            .NotEmpty();
        
        RuleFor(x => x.Role)
            .NotEmpty();
    }
}