using Domain.Models.Comments;
using Domain.Requests.Comments;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ICommentsService
{
    Task<Result<List<CommentModel>>> GetByCardAsync(int boardId, int cardId);
    
    Task<Result<CommentModel>> CreateAsync(int boardId, CreateCommentRequest request);
    
    Task<Result<object>> UpdateAsync(int boardId, int id, UpdateCommentRequest request);
    
    Task<Result<object>> DeleteAsync(int boardId, int id);
}