using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Invitations;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Commands.DeclinedInvitation;

internal class DeclineInvitationCommandHandler : 
    ICommandHandler<DeclineInvitationCommand, InvitationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeclineInvitationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<InvitationDto>> Handle(
        DeclineInvitationCommand request, 
        CancellationToken cancellationToken)
    {
        var invitationRepository = _unitOfWork.GetRepository<IInvitationRepository>();
        
        var invitation = await invitationRepository
            .GetByIdWithIncludedBoardAsync(request.InvitationId, cancellationToken);
        
        if (invitation == null)
        {
            return Result.Failure<InvitationDto>(InvitationErrors.InvitationDoesNotExist);
        }

        if (invitation.InvitedUserId != request.UserId)
        {
            return Result.Failure<InvitationDto>(InvitationErrors.InvitationDoesNotBelongsToUserError);
        }
        
        try
        {
            invitation.Status = InvitationStatus.Declined;
            invitation.RespondedAt = DateTime.UtcNow;
            
            invitationRepository.Update(invitation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var response = _mapper.Map<InvitationDto>(invitation);

            return response;
        }
        catch
        {
            return Result.Failure<InvitationDto>(InvitationErrors.UpdateInvitationError);
        }
    }
}