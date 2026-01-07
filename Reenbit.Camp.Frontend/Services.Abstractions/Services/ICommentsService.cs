using Domain.Models.Comments;
using Domain.Requests.Comments;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ICommentsService
{
    Task<Result<List<CommentModel>>> GetByCardAsync(int cardId);
    
    Task<Result<CommentModel>> CreateAsync(CreateCommentRequest request);
    
    Task<Result<object>> UpdateAsync(int id, UpdateCommentRequest request);
    
    Task<Result<object>> DeleteAsync(int id);
}