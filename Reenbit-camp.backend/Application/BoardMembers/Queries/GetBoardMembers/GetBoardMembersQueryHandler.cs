using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.BoardMembers;
using Domain.Repositories;
using Domain.Shared;

namespace Application.BoardMembers.Queries.GetBoardMembers;

internal class GetBoardMembersQueryHandler : IQueryHandler<GetBoardMembersQuery, List<BoardMemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBoardMembersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<BoardMemberDto>>> Handle(
        GetBoardMembersQuery request, 
        CancellationToken cancellationToken)
    {
        var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();
        
        var boardMembers = await boardMemberRepository
            .GetByBoardIdAsync(request.BoardId, cancellationToken);

        var response = _mapper.Map<List<BoardMemberDto>>(boardMembers);
        
        return response;
    }
}