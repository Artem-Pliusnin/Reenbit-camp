using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardLabels.Commands.DeleteCardLabel;

internal class DeleteCardLabelCommandHandler : ICommandHandler<DeleteCardLabelCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCardLabelCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        DeleteCardLabelCommand request, 
        CancellationToken cancellationToken)
    {
        var cardLabelsRepository = _unitOfWork.GetRepository<ICardLabelsRepository>();
        
        var cardLabel = await cardLabelsRepository
            .GetByIdAsync(request.CardLabelId, cancellationToken);

        if (cardLabel == null)
        {
            return Result.Failure<bool>(CardLabelErrors.CardLabelDoesNotExistError);
        }
        
        cardLabelsRepository.Remove(cardLabel);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}