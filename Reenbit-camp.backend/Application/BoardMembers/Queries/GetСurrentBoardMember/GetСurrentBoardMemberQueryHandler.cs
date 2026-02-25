using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.BoardMembers;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.BoardMembers.Queries.GetСurrentBoardMember;

internal class GetСurrentBoardMemberQueryHandler 
    : IQueryHandler<GetСurrentBoardMemberQuery, BoardMemberDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetСurrentBoardMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async  Task<Result<BoardMemberDto>> Handle(
        GetСurrentBoardMemberQuery request, 
        CancellationToken cancellationToken)
    {
        var boardRepository = _unitOfWork.GetRepository<IBoardRepository>();
        
        var board = await boardRepository
            .GetByIdAsync(request.BoardId, cancellationToken);

        if (board.Status != BoardStatus.Active)
        {
            return Result.Failure<BoardMemberDto>(BoardErrors.BoardIsNotActive);
        }
        
        var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();
        
        var boardMember = await boardMemberRepository
            .GetByUserAndBoardIdAsync(request.UserId, request.BoardId, cancellationToken);
        
        var response = _mapper.Map<BoardMemberDto>(boardMember);
        
        return response;
    }
}