using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Lists.Commands.UpdateListPosition;

internal class UpdateListPositionCommandHandler : ICommandHandler<UpdateListPositionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateListPositionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        UpdateListPositionCommand request,
        CancellationToken cancellationToken)
    {
        var listRepository = _unitOfWork.GetRepository<IListRepository>();
        
        var list = await listRepository
            .GetByIdAsync(request.ListId, cancellationToken);

        if (list == null)
        {
            return Result.Failure(ListErrors.ListDoesNotExistError);
        }

        try
        {
            await listRepository
                .MoveListAsync(
                    list.BoardId,
                    request.ListId, 
                    request.NewPosition, 
                    cancellationToken);
            
            list.LastUpdatedBy = request.UserId;
            list.LastUpdateDate = DateTime.UtcNow;
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("List.UpdatePositionFailure", ex.Message));
        }
    }
}