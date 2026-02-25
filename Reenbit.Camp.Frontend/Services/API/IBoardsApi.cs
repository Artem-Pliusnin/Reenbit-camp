using Domain.DTOs.Boards;
using Domain.Models.Boards;
using Domain.Requests.Boards;
using Domain.Responses.Boards;
using Domain.Responses.Shared;
using Refit;

namespace Services.API;

public interface IBoardsApi
{
    [Post("/Boards")]
    Task<ApiResponse<BoardCardDto>> CreateAsync(
        [Body] CreateBoardRequest request);

    [Get("/Boards")]
    Task<ApiResponse<PaginationDto<BoardCardDto>>> GetByUserAsync(
        [Query] BoardsFilterModel filter);
    
    [Get("/Boards/{boardId}")]
    Task<ApiResponse<BoardInfoDto>> GetInfoAsync(int boardId);

    [Put("/Boards/{boardId}")]
    Task<ApiResponse<object>> UpdateAsync(
        int boardId,
        [Body] UpdateBoardRequest request);
    
    [Get("/Boards/archived")]
    Task<ApiResponse<PaginationDto<BoardDto>>> GetArchivedByUserAsync(
        [Query] ArchivedBoardsFilter filter);
    
    [Post("/Boards/{boardId}/archive")]
    Task<ApiResponse<object>> ArchiveBoard(int boardId);
    
    [Post("/Boards/{boardId}/restore")]
    Task<ApiResponse<object>> RestoreBoard(int boardId);

    [Get("/Boards/can-create")]
    Task<ApiResponse<bool>> CanCreateBoardAsync();
}