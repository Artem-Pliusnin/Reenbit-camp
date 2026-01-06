using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Comments.Commands.UpdateCommet;

internal class UpdateCommetCommandHandler : ICommandHandler<UpdateCommetCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateCommetCommand request, 
        CancellationToken cancellationToken)
    {
        try
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
                return  Result.Failure(CommentErrors.CommentDoesNotBelongsToUserError);
            }
            
            comment.Text = request.Text;
            comment.IsEdited = true;

            commentsRepository.Update(comment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(CommentErrors.UpdateCommentError);
        }
    }
}