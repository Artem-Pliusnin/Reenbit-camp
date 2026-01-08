using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.BoardMembers.Commands.DeleteBoardMember;

internal class DeleteBoardMemberCommandHandler : ICommandHandler<DeleteBoardMemberCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteBoardMemberCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(DeleteBoardMemberCommand request, CancellationToken cancellationToken)
    {
        var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();
        
        var boardMember = await boardMemberRepository
            .GetByIdAsync(request.BoardMemberId, cancellationToken);

        if (boardMember == null)
        {
            return Result.Failure<bool>(BoardMemberErrors.BoardMemberDoesNotExistError);
        }
        
        boardMemberRepository.Remove(boardMember);
        
        var cardMembersRepository = _unitOfWork.GetRepository<ICardMembersRepository>();
        
        await cardMembersRepository
            .DeleteAllByUserAndBoard(
                boardMember.UserId, 
                boardMember.BoardId, 
                cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}