using Domain.Requests.Comments;
using Domain.Responses.Comments;
using Refit;

namespace Services.API;

public interface ICommentsApi
{
    [Get("/Comments/card/{id}")]
    Task<ApiResponse<List<CommentDto>>> GetByCardAsync(int id);

    [Post("/Comments")]
    Task<ApiResponse<CommentDto>> CreateAsync(
        [Body] CreateCommentRequest request);
    
    [Put("/Comments/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        [Body] UpdateCommentRequest request,
        int id);
    
    [Delete("/Comments/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int id);
}

