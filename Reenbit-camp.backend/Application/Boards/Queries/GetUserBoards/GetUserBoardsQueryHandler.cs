using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Boards;
using Domain.DTOs.Shared;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Boards.Queries.GetUserBoards;

internal class GetUserBoardsQueryHandlerv : IQueryHandler<GetUserBoardsQuery, PaginationDto<BoardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserBoardsQueryHandlerv(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginationDto<BoardDto>>> Handle(
        GetUserBoardsQuery request, 
        CancellationToken cancellationToken)
    {
        var boardRepository = _unitOfWork.GetRepository<IBoardRepository>();
        
        var paginationDto = await boardRepository
            .GetByUserIdAsync(request.UserId, request.Filter, cancellationToken);

        var boards = _mapper.Map<List<BoardDto>>(paginationDto.Dtos);
        
        return new PaginationDto<BoardDto>()
        {
            Dtos = boards,
            CurrentPage = paginationDto.CurrentPage,
            TotalPages = paginationDto.TotalPages,
        };
    }
}