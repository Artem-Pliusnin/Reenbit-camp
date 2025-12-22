using Domain.Requests.Lists;
using Domain.Responses.Lists;
using Refit;

namespace Services.API;

public interface IListsApi
{
    [Get("/Lists/board/{id}")]
    Task<ApiResponse<List<ListDto>>> GetByBoardAsync(int id);

    [Post("/Lists")]
    Task<ApiResponse<ListDto>> CreateAsync(
        [Body] CreateListRequest request);

    [Put("/Lists/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        [Body] UpdateListRequest request,
        int id);
    
    [Put("/Lists/{id}/position")]
    Task<ApiResponse<object>> UpdatePositionAsync(
        [Body] UpdateListPositionRequest request,
        int id);
    
    [Delete("/Lists/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int id);
}