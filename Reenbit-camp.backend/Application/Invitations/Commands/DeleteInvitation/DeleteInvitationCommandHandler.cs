using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Commands.DeleteInvitation;

internal class DeleteInvitationCommandHandler : ICommandHandler<DeleteInvitationCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteInvitationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<int>> Handle(DeleteInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitationsRepository = _unitOfWork.GetRepository<IInvitationRepository>();
        
        var invitation = await invitationsRepository
            .GetByIdAsync(request.InvitationId, cancellationToken);

        if (invitation == null)
        {
            return Result.Failure<int>(InvitationErrors.InvitationDoesNotExist);
        }
        
        invitationsRepository.Remove(invitation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(invitation.InvitedUserId);
    }
}