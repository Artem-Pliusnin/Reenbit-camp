using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Commands.DeleteCard;

internal class DeleteCardCommandHandler : ICommandHandler<DeleteCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var card = await cardRepository
            .GetByIdAsync(request.CardId, cancellationToken);

        if (card == null)
        {
            return Result.Failure<bool>(CardErrors.CardDoesNotExistError);
        }
        
        await cardRepository.DeleteCardAsync(
            card.Id, 
            card.ListId, 
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}