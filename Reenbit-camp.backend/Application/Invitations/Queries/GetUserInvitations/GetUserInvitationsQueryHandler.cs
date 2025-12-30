using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Invitations;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Queries.GetUserInvitations;

internal class GetUserInvitationsQueryHandler :
    IQueryHandler<GetUserInvitationsQuery, List<InvitationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserInvitationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<InvitationDto>>> Handle(
        GetUserInvitationsQuery request, 
        CancellationToken cancellationToken)
    {
        var invotationRepository = _unitOfWork.GetRepository<IInvitationRepository>();
        
        var invitations = await invotationRepository
            .GetByUserIdAsync(request.UserId);
        
        var response = _mapper.Map<List<InvitationDto>>(invitations);
        
        return response;
    }
}