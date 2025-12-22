using Domain.Requests.Cards;
using Domain.Requests.Lists;
using Domain.Responses.Cards;
using Domain.Responses.Lists;
using Refit;

namespace Services.API;

public interface ICardsApi
{
    [Get("/Cards/list/{id}")]
    Task<ApiResponse<List<CardDto>>> GetByListAsync(int id);

    [Post("/Cards")]
    Task<ApiResponse<CardDto>> CreateAsync(
        [Body] CreateCardRequest request);

    [Put("/Cards/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        [Body] UpdateCardRequest request,
        int id);
    
    [Put("/Cards/{id}/position")]
    Task<ApiResponse<object>> UpdatePositionAsync(
        [Body] UpdateCardPositionRequest request,
        int id);
    
    [Put("/Cards/{id}/deadline")]
    Task<ApiResponse<object>> UpdateDeadlineAsync(
        [Body] UpdateCardDeadlineRequest request,
        int id);
    
    [Delete("/Cards/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int id);
}