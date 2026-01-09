using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Boards;
using Domain.DTOs.Invitations;
using Domain.DTOs.Users;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Commands.CreateInvitation;

internal class CreateInvitationCommandHandler : 
    ICommandHandler<CreateInvitationCommand, InvitationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateInvitationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<InvitationDto>> Handle(
        CreateInvitationCommand request, 
        CancellationToken cancellationToken)
    {
        if (request.InvitedByUserId == request.InvitedUserId)
        {
            return Result.Failure<InvitationDto>(InvitationErrors.TheSameUserError);
        }
        try
        {
            var invitationRepository = _unitOfWork.GetRepository<IInvitationRepository>();

            var invitation = new Invitation()
            {
                BoardId = request.BoardId,
                InvitedUserId = request.InvitedUserId,
                InvitedByUserId = request.InvitedByUserId,
                Status = InvitationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };
            
            invitationRepository.Add(invitation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var fullInvitation = await invitationRepository
                .GetByIdWithIncludesAsync(invitation.Id, cancellationToken);
            
            var response = _mapper.Map<InvitationDto>(fullInvitation);

            return response;
        }
        catch
        {
            return Result.Failure<InvitationDto>(InvitationErrors.CreateInvitationError);
        }
    }
}