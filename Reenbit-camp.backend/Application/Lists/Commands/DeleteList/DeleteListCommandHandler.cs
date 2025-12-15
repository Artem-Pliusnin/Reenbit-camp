using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Lists.Commands.DeleteList;

internal class DeleteListCommandHandler : ICommandHandler<DeleteListCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteListCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(DeleteListCommand request, CancellationToken cancellationToken)
    {
        var listRepository = _unitOfWork.GetRepository<IListRepository>();
        
        var list = await listRepository.GetByIdAsync(request.ListId);

        if (list == null)
        {
            return Result.Failure<bool>(ListErrors.ListDoesNotExistError);
        }
        
        listRepository.Remove(list);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
        
    }
}