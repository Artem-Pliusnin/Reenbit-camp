using Domain.Requests.Invitations;
using Domain.Requests.Labels;
using Domain.Responses.Invitations;
using Domain.Responses.Labels;
using Domain.Shared;
using Refit;

namespace Services.API;

public interface ILabelsApi
{
    [Get("/Labels/board/{id}")]
    Task<ApiResponse<List<LabelDto>>> GetByBoardAsync(int id);
    
    [Get("/Labels/not-connected/card/{id}")]
    Task<ApiResponse<List<LabelDto>>> GetNotConnectedAsync(int id);

    [Post("/Labels")]
    Task<ApiResponse<LabelDto>> CreateAsync(
        [Body] CreateLabelRequest request);
    
    [Put("/Labels/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        [Body] UpdateLabelRequest request,
        int id);
    
    [Delete("/Labels/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int id);
}
