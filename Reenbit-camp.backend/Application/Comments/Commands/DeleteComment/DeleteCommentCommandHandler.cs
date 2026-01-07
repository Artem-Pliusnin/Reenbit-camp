using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Comments.Commands.DeleteComment;

internal class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCommentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        DeleteCommentCommand request, 
        CancellationToken cancellationToken)
    {
        var commentsRepository = _unitOfWork.GetRepository<ICommentsRepository>();
        
        var comment = await commentsRepository
            .GetByIdAsync(request.CommentId, cancellationToken);

        if (comment == null)
        {
            return Result.Failure(CommentErrors.CommentDoesNotExist);
        }

        if (comment.UserId != request.UserId)
        {
            return Result.Failure(CommentErrors.CommentDoesNotBelongsToUserError);
        }
        
        commentsRepository.Remove(comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}