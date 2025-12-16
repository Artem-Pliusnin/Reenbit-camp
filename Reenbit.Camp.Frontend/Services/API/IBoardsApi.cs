using Domain.Models.Boards;
using Domain.Requests.Boards;
using Domain.Responses.Boards;
using Domain.Responses.Shared;
using Refit;

namespace Services.API;

public interface IBoardsApi
{
    [Post("/Boards")]
    Task<ApiResponse<BoardDto>> CreateAsync(
        [Body] CreateBoardRequest request);

    [Get("/Boards")]
    Task<ApiResponse<PaginationDto<BoardDto>>> GetByUserAsync(
        [Query] BoardsFilterModel filter);

    [Put("/Boards/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        int id,
        [Body] UpdateBoardRequest request);
}