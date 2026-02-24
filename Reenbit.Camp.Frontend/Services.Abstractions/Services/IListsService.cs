using Domain.Models.Lists;
using Domain.Requests.Lists;
using Domain.Responses.Lists;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IListsService
{
    Task<Result<ListModel>> CreateAsync(int boardId, CreateListRequest request);
    
    Task<Result<List<ListModel>>> GetByBoardAsync(int boardId);
    
    Task<Result<object>> UpdateAsync(int boardId, int id, UpdateListRequest request);
    
    Task<Result<object>> UpdatePositionAsync(int boardId, int id, UpdateListPositionRequest request);
    
    Task<Result<object>> DeleteAsync(int boardId, int id);
}