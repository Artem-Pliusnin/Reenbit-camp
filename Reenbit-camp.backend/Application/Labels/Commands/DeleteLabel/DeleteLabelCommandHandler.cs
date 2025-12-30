using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Labels.Commands.DeleteLabel;

internal class DeleteLabelCommandHandler : ICommandHandler<DeleteLabelCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLabelCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        DeleteLabelCommand request, 
        CancellationToken cancellationToken)
    {
        var labelRepository = _unitOfWork.GetRepository<ILabelRepository>();
        
        var label = await labelRepository
            .GetByIdAsync(request.LabelId, cancellationToken);

        if (label == null)
        {
            return Result.Failure(LabelErrors.LabelDoesNotExistError);
        }
        
        labelRepository.Remove(label);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
        
    }
}