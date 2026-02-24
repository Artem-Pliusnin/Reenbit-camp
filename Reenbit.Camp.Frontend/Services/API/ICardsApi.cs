using Domain.Requests.Cards;
using Domain.Requests.Lists;
using Domain.Responses.Cards;
using Domain.Responses.Lists;
using Refit;

namespace Services.API;

public interface ICardsApi
{
    [Get("/Board/{boardId}/Cards/list/{id}")]
    Task<ApiResponse<List<CardDto>>> GetByListAsync(int boardId, int id);
    
    [Get("/Board/{boardId}/Cards/{id}")]
    Task<ApiResponse<CardInfoDto>> GetInfoAsync(int boardId, int id);

    [Post("/Board/{boardId}/Cards")]
    Task<ApiResponse<CardDto>> CreateAsync(
        int boardId,
        [Body] CreateCardRequest request);

    [Put("/Board/{boardId}/Cards/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        [Body] UpdateCardRequest request,
        int boardId,
        int id);
    
    [Put("/Board/{boardId}/Cards/{id}/status")]
    Task<ApiResponse<object>> UpdateStatusAsync(
        [Body] UpdateCardStatusRequest request,
        int boardId,
        int id);
    
    [Put("/Board/{boardId}/Cards/{id}/position")]
    Task<ApiResponse<object>> UpdatePositionAsync(
        [Body] UpdateCardPositionRequest request,
        int boardId,
        int id);
    
    [Put("/Board/{boardId}/Cards/{id}/deadline")]
    Task<ApiResponse<object>> UpdateDeadlineAsync(
        [Body] UpdateCardDeadlineRequest request,
        int boardId,
        int id);
    
    [Delete("/Board/{boardId}/Cards/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int boardId, int id);
}