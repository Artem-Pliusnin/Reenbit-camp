using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Invitations;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Commands.DeleteInvitation;

internal class DeleteInvitationCommandHandler : ICommandHandler<DeleteInvitationCommand, InvitationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteInvitationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<InvitationDto>> Handle(DeleteInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitationsRepository = _unitOfWork.GetRepository<IInvitationRepository>();
        
        var invitation = await invitationsRepository
            .GetByIdWithIncludesAsync(request.InvitationId, cancellationToken);

        if (invitation == null)
        {
            return Result.Failure<InvitationDto>(InvitationErrors.InvitationDoesNotExist);
        }
        
        var response = _mapper.Map<InvitationDto>(invitation);
        
        invitationsRepository.Remove(invitation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return response;
    }
}