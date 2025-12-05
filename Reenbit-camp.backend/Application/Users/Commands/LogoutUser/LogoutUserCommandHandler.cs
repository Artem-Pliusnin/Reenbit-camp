using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.LogoutUser;

public class LogoutUserCommandHandler : ICommandHandler<LogoutUserCommand, bool>
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
            .GetSessionByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (session == null)
        {
            return Result.Failure<bool>(SessionErrors.InvalidRefreshToken);
        }

        if (session.UserId != request.UserId)
        {
            return Result.Failure<bool>(SessionErrors.RefreshTokenUserMismatch);
        }
        
        sessionRepository.Remove(session);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(true);
    }
}