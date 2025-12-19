using Domain.Models.Lists;
using Domain.Requests.Lists;
using Domain.Responses.Lists;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IListsService
{
    Task<Result<ListModel>> CreateAsync(CreateListRequest request);
    
    Task<Result<List<ListModel>>> GetByBoardAsync(int boardId);
    
    Task<Result<object>> UpdateAsync(int id, UpdateListRequest request);
    
    Task<Result<object>> UpdatePositionAsync(int id, UpdateListPositionRequest request);
    
    Task<Result<object>> DeleteAsync(int id);
}