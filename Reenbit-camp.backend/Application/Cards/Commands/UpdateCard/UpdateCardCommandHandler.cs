using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Commands.UpdateCard;

internal class UpdateCardCommandHandler : ICommandHandler<UpdateCardCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        UpdateCardCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var cardRepository = _unitOfWork.GetRepository<ICardRepository>();

            var card = await cardRepository
                .GetByIdAsync(request.CardId, cancellationToken);

            if (card == null)
            {
                return Result.Failure(CardErrors.CardDoesNotExistError);
            }

            card.Title = request.Title;
            card.Description = request.Description;
            card.LastUpdatedBy = request.UserId;
            card.LastUpdateDate = DateTime.UtcNow;

            cardRepository.Update(card);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(CardErrors.UpdateCardError);
        }
    }
}