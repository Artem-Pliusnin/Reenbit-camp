using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.CardMembers.Commands.DeleteCardMember;

internal class DeleteCardMemberCommandHandler : ICommandHandler<DeleteCardMemberCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCardMemberCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        DeleteCardMemberCommand request, 
        CancellationToken cancellationToken)
    {
        var cardMembersRepository = _unitOfWork.GetRepository<ICardMembersRepository>();
        
        var cardMember = await cardMembersRepository
            .GetByIdAsync(request.CardMemberId, cancellationToken);

        if (cardMember == null)
        {
            return Result.Failure<bool>(CardMemberErrors.CardMemberDoesNotExistError);
        }
        
        cardMembersRepository.Remove(cardMember);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}