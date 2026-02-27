using Domain.Requests.Lists;
using Domain.Responses.Lists;
using Refit;

namespace Services.API;

public interface IListsApi
{
    [Get("/Board/{boardId}/Lists")]
    Task<ApiResponse<List<ListDto>>> GetByBoardAsync(int boardId);

    [Post("/Board/{boardId}/Lists")]
    Task<ApiResponse<ListDto>> CreateAsync(
        [Body] CreateListRequest request,
        int boardId);

    [Put("/Board/{boardId}/Lists/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        [Body] UpdateListRequest request,
        int boardId,
        int id);
    
    [Put("/Board/{boardId}/Lists/{id}/position")]
    Task<ApiResponse<object>> UpdatePositionAsync(
        [Body] UpdateListPositionRequest request,
        int boardId,
        int id);
    
    [Delete("/Board/{boardId}/Lists/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int boardId, int id);
}