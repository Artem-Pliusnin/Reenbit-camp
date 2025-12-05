using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.DTOs.Authorization;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.RefreshTokens;

public class RefreshTokensCommandHandler : ICommandHandler<RefreshTokensCommand, TokensResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;

    public RefreshTokensCommandHandler(
        IUnitOfWork unitOfWork, 
        ITokenProvider tokenProvider)
    {
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Result<TokensResponseDto>> Handle(
        RefreshTokensCommand request, 
        CancellationToken cancellationToken)
    {
        var sessionRepository = _unitOfWork.GetRepository<ISessionRepository>();
        
        var session = await sessionRepository
            .GetSessionByRefreshToken(request.RefreshToken, cancellationToken);

        if (session == null || session.ExpiresOn <= DateTime.UtcNow)
        {
            return Result.Failure<TokensResponseDto>
                (SessionErrors.InvalidRefreshToken);
        }
        
        var newAccessToken = _tokenProvider.CreateToken(session.User);
        var newRefreshToken = _tokenProvider.GenerateRefreshToken();

        session.Token = newRefreshToken;
        session.ExpiresOn = DateTime.UtcNow
            .AddDays(_tokenProvider.RefreshTokenLifetimeDays);
        
        sessionRepository.Update(session);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new TokensResponseDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = DateTime.UtcNow
                .AddMinutes(_tokenProvider.AccessTokenLifetimeMinutes),
            RefreshTokenExpiresAt = session.ExpiresOn,
        };
        
        return Result.Success(response);
    }
}