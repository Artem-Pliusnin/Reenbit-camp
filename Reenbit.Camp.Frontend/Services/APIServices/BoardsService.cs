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

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            var newBoard = _mapper.Map<BoardModel>(response.Content);
            return newBoard;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<BoardModel>(error);
    }
    
    public async Task<Result<BoardInfoModel>> GetInfoAsync(int boardId)
    {
        var response = await _boardsApi.GetInfoAsync(boardId);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            var boardInfoModel = _mapper.Map<BoardInfoModel>(response.Content);
            
            return boardInfoModel;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<BoardInfoModel>(error);
    }

    public async Task<Result<PaginationDto<BoardModel>>> GetByUserAsync(BoardsFilterModel filter)
    {
        var response = await _boardsApi.GetByUserAsync(filter);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            var paginationDto = new PaginationDto<BoardModel>(
                _mapper.Map<List<BoardModel>>(response.Content.Dtos),
                response.Content.CurrentPage,
                response.Content.TotalPages
            );
            
            return paginationDto;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<PaginationDto<BoardModel>>(error);
    }

    public async Task<Result<object>> UpdateAsync(int id, UpdateBoardRequest request)
    {
        var response = await _boardsApi.UpdateAsync(id, request);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<object>(error);
    }
}