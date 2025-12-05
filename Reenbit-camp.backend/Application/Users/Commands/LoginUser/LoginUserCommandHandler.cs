using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.DTOs.Authorization;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler 
    : ICommandHandler<LoginUserCommand, LoginResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenProvider tokenProvider,
        IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Result<LoginResponseDto>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        var sessionRepository = _unitOfWork.GetRepository<ISessionRepository>();
        
        var user = await userRepository
            .GetByEmailAsync(request.Email, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure<LoginResponseDto>(UserErrors.NotFoundByEmail);
        }
        
        var verified = _passwordHasher.Verify(request.Password, user.Password);

        if (!verified)
        {
            return Result.Failure<LoginResponseDto>(UserErrors.NotFoundByEmail);
        }
        
        var accessToken = _tokenProvider.CreateToken(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken();

        var session = new Session()
        {
            Token = refreshToken,
            ExpiresOn = DateTime.UtcNow
                .AddDays(_tokenProvider.RefreshTokenLifetimeDays),
            UserId = user.Id,
        };
        
        sessionRepository.Add(session);

        var response = new LoginResponseDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = DateTime.UtcNow
                .AddMinutes(_tokenProvider.AccessTokenLifetimeMinutes),
            RefreshTokenExpiresAt = session.ExpiresOn,
        };
        
        return Result.Success(response);
    }
}