using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.DTOs.Authorization;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.UpdateUserPassword;

internal class UpdateUserPasswordCommandHandler : ICommandHandler<UpdateUserPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserPasswordCommandHandler(
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(
        UpdateUserPasswordCommand request, 
        CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        
        var user = await userRepository
            .GetByIdAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure(UserErrors.UserDoesNotExistError);
        }

        if (user.Password is not null)
        {
            var verified = _passwordHasher.Verify(request.Password, user.Password);

            if (!verified)
            {
                return Result.Failure<TokensResponseDto>(UserErrors.InvalidCredentials);
            }
        }
        
        user.Password = _passwordHasher.Hash(request.NewPassword);
        
        userRepository.Update(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}