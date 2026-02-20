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
    
    [Get("/Boards/{id}")]
    Task<ApiResponse<BoardInfoDto>> GetInfoAsync(int id);

    [Put("/Boards/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        int id,
        [Body] UpdateBoardRequest request);
    
    [Get("/Boards/archived")]
    Task<ApiResponse<PaginationDto<BoardDto>>> GetArchivedByUserAsync(
        [Query] ArchivedBoardsFilter filter);
    
    [Post("/Boards/{id}/archive")]
    Task<ApiResponse<object>> ArchiveBoard(int id);
    
    [Post("/Boards/{id}/restore")]
    Task<ApiResponse<object>> RestoreBoard(int id);

    [Get("/Boards/can-create")]
    Task<ApiResponse<bool>> CanCreateBoardAsync();
}