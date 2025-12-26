using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Commands.UpdateCardStatus;

public class UpdateCardStatusCommandHandler : ICommandHandler<UpdateCardStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCardStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(UpdateCardStatusCommand request, CancellationToken cancellationToken)
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
            card.IsCompleted = request.IsCompleted;
            card.LastUpdatedBy = request.UserId;
            card.LastUpdateDate = DateTime.UtcNow;
            
            cardRepository.Update(card);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("Card.UpdateStatusFailure", ex.Message));
        }
    }
}