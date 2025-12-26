using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Commands.AcceptInvitation;

internal class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AcceptInvitationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        AcceptInvitationCommand request, 
        CancellationToken cancellationToken)
    {
        var invitationRepository = _unitOfWork.GetRepository<IInvitationRepository>();
        
        var invitation = await invitationRepository.GetByIdAsync(request.InvitationId);
        
        if (invitation == null)
        {
            return Result.Failure(InvitationErrors.InvitationDoesNotExist);
        }

        if (invitation.InvitedUserId != request.UserId)
        {
            return Result.Failure(InvitationErrors.InvitationDoesNotBelongsToUserError);
        }
        
        try
        {
            invitation.Status = InvitationStatus.Accepted;
            invitation.RespondedAt = DateTime.UtcNow;
            
            invitationRepository.Update(invitation);
            
            var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();

            var boardMember = new BoardMember()
            {
                BoardId = invitation.BoardId,
                UserId = invitation.InvitedUserId,
                Role = BoardRole.Member
            };

            boardMemberRepository.Add(boardMember);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(InvitationErrors.UpdateInvitationError);
        }
    }
}