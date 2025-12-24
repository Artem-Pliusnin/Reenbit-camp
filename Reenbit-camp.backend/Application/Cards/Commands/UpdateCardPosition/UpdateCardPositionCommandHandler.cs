using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Commands.UpdateCardPosition;

internal class UpdateCardPositionCommandHandler : ICommandHandler<UpdateCardPositionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCardPositionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(UpdateCardPositionCommand request, CancellationToken cancellationToken)
    {
        var cardRepository = _unitOfWork.GetRepository<ICardRepository>();
        
        var card = await cardRepository
            .GetByIdAsync(request.CardId, cancellationToken);

        if (card == null)
        {
            return Result.Failure(CardErrors.CardDoesNotExistError);
        }

        try
        {
            await cardRepository
                .MoveCardAsync(
                    card.Id,
                    request.NewListId, 
                    request.NewPosition, 
                    cancellationToken);
            
            card.LastUpdatedBy = request.UserId;
            card.LastUpdateDate = DateTime.UtcNow;
            
            cardRepository.Update(card);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("Card.UpdatePositionFailure", ex.Message));
        }
    }
}