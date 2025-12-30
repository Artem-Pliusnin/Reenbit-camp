using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.BoardMembers.Commands.UpdateBoardMemberRole;

public class UpdateBoardMemberRoleCommandHandler : ICommandHandler<UpdateBoardMemberRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBoardMemberRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
        UpdateBoardMemberRoleCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();
        
            var boardMember = await boardMemberRepository.GetByIdAsync(request.BoardMemberId);

            if (boardMember == null)
            {
                return Result.Failure(BoardMemberErrors.BoardMemberDoesNotExistError);
            }

            boardMember.Role = request.Role;
            
            boardMemberRepository.Update(boardMember);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(BoardMemberErrors.UpdateBoardMemberError);
        }
    }
}