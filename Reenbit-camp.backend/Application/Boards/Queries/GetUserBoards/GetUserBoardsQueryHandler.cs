using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Boards;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Queries.GetUserBoards;

public class GetUserBoardsQueryHandlerv : IQueryHandler<GetUserBoardsQuery, List<BoardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserBoardsQueryHandlerv(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<BoardDto>>> Handle(
        GetUserBoardsQuery request, 
        CancellationToken cancellationToken)
    {
        var boardRepository = _unitOfWork.GetRepository<IBoardRepository>();
        
        var boards = await boardRepository
            .GetByUserIdAsync(request.UserId, cancellationToken);

        var response = _mapper.Map<List<BoardDto>>(boards);
        
        return response;
    }
}