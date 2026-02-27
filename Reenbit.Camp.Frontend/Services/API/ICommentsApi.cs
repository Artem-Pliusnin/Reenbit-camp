using Domain.Requests.Comments;
using Domain.Responses.Comments;
using Refit;

namespace Services.API;

public interface ICommentsApi
{
    [Get("/Board/{boardId}/Comments/card/{id}")]
    Task<ApiResponse<List<CommentDto>>> GetByCardAsync(int boardId, int id);

    [Post("/Board/{boardId}/Comments")]
    Task<ApiResponse<CommentDto>> CreateAsync(
        [Body] CreateCommentRequest request,
        int boardId);
    
    [Put("/Board/{boardId}/Comments/{id}")]
    Task<ApiResponse<object>> UpdateAsync(
        [Body] UpdateCommentRequest request,
        int boardId,
        int id);
    
    [Delete("/Board/{boardId}/Comments/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int boardId, int id);
}

