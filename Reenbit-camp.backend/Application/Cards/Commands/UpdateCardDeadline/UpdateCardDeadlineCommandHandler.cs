using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Cards.Commands.UpdateCardDeadline;

internal class UpdateCardDeadlineCommandHandler : ICommandHandler<UpdateCardDeadlineCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCardDeadlineCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        UpdateCardDeadlineCommand request, 
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

            card.StartDate = request.StartDate;
            card.DueDate = request.DueDate;
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