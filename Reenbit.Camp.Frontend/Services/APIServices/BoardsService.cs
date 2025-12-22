using AutoMapper;
using Domain.Models.Boards;
using Domain.Requests.Boards;
using Domain.Responses.Boards;
using Domain.Responses.Shared;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class BoardsService : IBoardsService
{
    private readonly IBoardsApi _boardsApi;
    private readonly IMapper _mapper;

    public BoardsService(IBoardsApi boardsApi, IMapper mapper)
    {
        _boardsApi = boardsApi;
        _mapper = mapper;
    }
    
    public async Task<Result<BoardModel>> CreateAsync(CreateBoardRequest request)
    {
        var response = await _boardsApi.CreateAsync(request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<BoardModel>(content));
    }
    
    public async Task<Result<BoardInfoModel>> GetInfoAsync(int boardId)
    {
        var response = await _boardsApi.GetInfoAsync(boardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<BoardInfoModel>(content));
    }

    public async Task<Result<PaginationDto<BoardModel>>> GetByUserAsync(BoardsFilterModel filter)
    {
        var response = await _boardsApi.GetByUserAsync(filter);

        return response.HandleResultWithMapping(content 
            => new PaginationDto<BoardModel>(
            _mapper.Map<List<BoardModel>>(content.Dtos),
            content.CurrentPage,
            content.TotalPages
        ));
    }

    public async Task<Result<object>> UpdateAsync(int id, UpdateBoardRequest request)
    {
        var response = await _boardsApi.UpdateAsync(id, request);

        return response.HandleResult();
    }
}