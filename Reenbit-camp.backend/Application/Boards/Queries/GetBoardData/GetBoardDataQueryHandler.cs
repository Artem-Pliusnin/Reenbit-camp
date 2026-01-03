using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Boards;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Queries.GetBoardData;

internal class GetBoardDataQueryHandler 
    : IQueryHandler<GetBoardDataQuery, BoardInfoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBoardDataQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<BoardInfoDto>> Handle(
        GetBoardDataQuery request, 
        CancellationToken cancellationToken)
    {
        var boardRepository = _unitOfWork.GetRepository<IBoardRepository>();
        
        var board = await boardRepository
            .GetFullInfoAsync(request.BoardId, cancellationToken);

        if (board == null)
        {
            return Result
                .Failure<BoardInfoDto>(BoardErrors.BoardDoesNotExistError);
        }
        
        var boardDto = _mapper.Map<BoardInfoDto>(board);

        return boardDto;
    }
}