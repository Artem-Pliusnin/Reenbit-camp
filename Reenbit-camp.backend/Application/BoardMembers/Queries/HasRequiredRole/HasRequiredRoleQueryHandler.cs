using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.BoardMembers.Queries.HasRequiredRole;

internal class HasRequiredRoleQueryHandler : IQueryHandler<HasRequiredRoleQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public HasRequiredRoleQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<bool>> Handle(
        HasRequiredRoleQuery request, 
        CancellationToken cancellationToken)
    {
        var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();
        
        var member = await boardMemberRepository
            .GetByUserAndBoardIdAsync(request.UserId, request.BoardId, cancellationToken);

        if (member is null)
        {
            return Result.Failure<bool>(BoardMemberErrors.BoardMemberDoesNotExistError);
        }

        return member.Role <= request.MinimumRole;
    }
}