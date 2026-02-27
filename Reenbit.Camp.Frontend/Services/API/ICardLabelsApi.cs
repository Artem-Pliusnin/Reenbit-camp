using Domain.Requests.Labels;
using Domain.Responses.Labels;
using Refit;

namespace Services.API;

public interface ICardLabelsApi
{
    [Get("/Board/{boardId}/CardLabels/card/{id}")]
    Task<ApiResponse<List<CardLabelDto>>> GetByCardAsync(int boardId, int id);

    [Post("/Board/{boardId}/CardLabels")]
    Task<ApiResponse<CardLabelDto>> CreateAsync(
        int boardId,
        [Body] CreateCardLabelRequest request);
    
    [Delete("/Board/{boardId}/CardLabels/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int boardId, int id);
}
