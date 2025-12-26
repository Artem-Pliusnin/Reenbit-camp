using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Commands.DeclinedInvitation;

internal class DeclinedInvitationCommandHandler : ICommandHandler<DeclinedInvitationCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeclinedInvitationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        DeclinedInvitationCommand request, 
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
            invitation.Status = InvitationStatus.Declined;
            invitation.RespondedAt = DateTime.UtcNow;
            
            invitationRepository.Update(invitation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(InvitationErrors.UpdateInvitationError);
        }
    }
}