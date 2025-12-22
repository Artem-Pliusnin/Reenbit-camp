using AutoMapper;
using Domain.Models.Lists;
using Domain.Requests.Lists;
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
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<ListModel>(content));
    }

    public async Task<Result<List<ListModel>>> GetByBoardAsync(int boardId)
    {
        var response = await _listsApi.GetByBoardAsync(boardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<ListModel>>(content));
    }

    public async Task<Result<object>> UpdateAsync(int id, UpdateListRequest request)
    {
        var response = await _listsApi.UpdateAsync(request, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> UpdatePositionAsync(int id, UpdateListPositionRequest request)
    {
        var response = await _listsApi.UpdatePositionAsync(request, id);

        return response.HandleResult();
    }

    public async Task<Result<object>> DeleteAsync(int id)
    {
        var response = await _listsApi.DeleteAsync(id);

        return response.HandleResult();
    }
}