using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Auth.Commands.LogoutUser;

internal class LogoutUserCommandHandler : ICommandHandler<LogoutUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(
        LogoutUserCommand request,
        CancellationToken cancellationToken)
    {
        var sessionRepository = _unitOfWork.GetRepository<ISessionRepository>();
        
        var session = await sessionRepository
            .GetSessionByUserIdAsync(request.UserId, cancellationToken);

        if (session == null)
        {
            return Result.Failure<bool>(SessionErrors.NotFound);
        }
        
        sessionRepository.Remove(session);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}