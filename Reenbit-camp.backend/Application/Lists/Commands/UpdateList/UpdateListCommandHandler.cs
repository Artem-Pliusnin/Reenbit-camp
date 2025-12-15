using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Lists.Commands.UpdateList;

internal class UpdateListCommandHandler : ICommandHandler<UpdateListCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateListCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(UpdateListCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var listRepository = _unitOfWork.GetRepository<IListRepository>();

            var list = await listRepository
                .GetByIdAsync(request.ListId, cancellationToken);

            if (list == null)
            {
                return Result.Failure(ListErrors.ListDoesNotExistError);
            }

            list.Title = request.Title;
            list.LastUpdatedBy = request.UserId;
            list.LastUpdateDate = DateTime.UtcNow;

            listRepository.Update(list);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(ListErrors.UpdateListError);
        }
    }
}