using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Invitations;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Queries.GetInvitationsByBoard;


public class GetInvitationsByBoardQueryHandler : 
    IQueryHandler<GetInvitationsByBoardQuery, List<InvitationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetInvitationsByBoardQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<InvitationDto>>> Handle(
        GetInvitationsByBoardQuery request, 
        CancellationToken cancellationToken)
    {
        var invotationRepository = _unitOfWork.GetRepository<IInvitationRepository>();
        
        var invitations = await invotationRepository
            .GetBoardPendingAsync(request.BoardId, cancellationToken);
        
        var response = _mapper.Map<List<InvitationDto>>(invitations);
        
        return response;
    }
}