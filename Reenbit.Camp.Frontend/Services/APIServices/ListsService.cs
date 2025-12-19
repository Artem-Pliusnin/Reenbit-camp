using AutoMapper;
using Domain.Models.Boards;
using Domain.Models.Lists;
using Domain.Requests.Lists;
using Domain.Responses.Lists;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class ListsService : IListsService
{
    private readonly IListsApi _listsApi;
    private readonly IMapper _mapper;

    public ListsService(IListsApi listsApi, IMapper mapper)
    {
        _listsApi = listsApi;
        _mapper = mapper;
    }
    
    public async Task<Result<ListModel>> CreateAsync(CreateListRequest request)
    {
        var response = await _listsApi.CreateAsync(request);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            var newList = _mapper.Map<ListModel>(response.Content);
            return newList;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<ListModel>(error);
    }

    public async Task<Result<List<ListModel>>> GetByBoardAsync(int boardId)
    {
        var response = await _listsApi.GetByBoardAsync(boardId);

        if (response.IsSuccessStatusCode && response.Content != null)
        {
            var lists = _mapper.Map<List<ListModel>>(response.Content);
            return lists;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<List<ListModel>>(error);
    }

    public async Task<Result<object>> UpdateAsync(int id, UpdateListRequest request)
    {
        var response = await _listsApi.UpdateAsync(request, id);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<object>(error);
    }

    public async Task<Result<object>> UpdatePositionAsync(int id, UpdateListPositionRequest request)
    {
        var response = await _listsApi.UpdatePositionAsync(request, id);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<object>(error);
    }

    public async Task<Result<object>> DeleteAsync(int id)
    {
        var response = await _listsApi.DeleteAsync(id);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }
        
        var error = response.GetApiErrorAsync();
        return Result.Failure<object>(error);
    }
}