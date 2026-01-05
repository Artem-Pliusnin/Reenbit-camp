using Domain.Requests.Labels;
using Domain.Responses.Labels;
using Refit;

namespace Services.API;

public interface ICardLabelsApi
{
    [Get("/CardLabels/card/{id}")]
    Task<ApiResponse<List<CardLabelDto>>> GetByCardAsync(int id);

    [Post("/CardLabels")]
    Task<ApiResponse<CardLabelDto>> CreateAsync(
        [Body] CreateCardLabelRequest request);
    
    [Delete("/CardLabels/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int id);
}
