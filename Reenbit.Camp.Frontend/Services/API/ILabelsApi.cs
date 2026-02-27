using Domain.Requests.Invitations;
using Domain.Requests.Labels;
using Domain.Responses.Invitations;
using Domain.Responses.Labels;
using Domain.Shared;
using Refit;

namespace Services.API;

public interface ILabelsApi
{
    [Get("/Board/{boardId}/Labels")]
    Task<ApiResponse<List<LabelDto>>> GetByBoardAsync(int boardId);
    
    [Get("/Board/{boardId}/Labels/not-connected/card/{id}")]
    Task<ApiResponse<List<LabelDto>>> GetNotConnectedAsync(int boardId, int id);

    [Post("/Board/{boardId}/Labels")]
    Task<ApiResponse<LabelDto>> CreateAsync(
        [Body] CreateLabelRequest request,
        int boardId);
    
    [Put("/Board/{boardId}/Labels/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        [Body] UpdateLabelRequest request,
        int boardId,
        int id);
    
    [Delete("/Board/{boardId}/Labels/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int boardId, int id);
}
