using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.DTOs.Authorization;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Auth.Commands.GoogleLogin;

internal class GoogleLoginCommandHandler : ICommandHandler<GoogleLoginCommand, TokensResponseDto> 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;

    public GoogleLoginCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider)
    {
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Result<TokensResponseDto>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        var sessionRepository = _unitOfWork.GetRepository<ISessionRepository>();
        var subscriptionPlansRepository = _unitOfWork.GetRepository<ISubscriptionPlansRepository>();
        var userSubscriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();

        var user = await userRepository
            .GetByEmailAsync(request.Email, cancellationToken);
        
        if (user is null)
        {
            user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = null,
            };

            userRepository.Add(user);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var freePlan = await subscriptionPlansRepository
                .GetBySubscriptionName("Free", cancellationToken);

            userSubscriptionsRepository.Add(new UserSubscription
            {
                UserId = user.Id,
                SubscriptionPlanId = freePlan?.Id ?? 1,
                SubscriptionStatus = SubscriptionStatus.Active,
                CurrentPeriodEnd = null
            });
        }

        var accessToken = _tokenProvider.CreateToken(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken();

        var session = await sessionRepository
            .GetSessionByUserIdAsync(user.Id, cancellationToken);

        if (session is not null)
        {
            session.Token = refreshToken;
            session.ExpiresOn = DateTime.UtcNow
                .AddDays(_tokenProvider.RefreshTokenLifetimeDays);
            sessionRepository.Update(session);
        }
        else
        {
            sessionRepository.Add(new Session
            {
                Token = refreshToken,
                ExpiresOn = DateTime.UtcNow
                    .AddDays(_tokenProvider.RefreshTokenLifetimeDays),
                UserId = user.Id,
            });
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new TokensResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = DateTime.UtcNow
                .AddMinutes(_tokenProvider.AccessTokenLifetimeMinutes),
            RefreshTokenExpiresAt = DateTime.UtcNow
                .AddDays(_tokenProvider.RefreshTokenLifetimeDays),
        });
    }
}