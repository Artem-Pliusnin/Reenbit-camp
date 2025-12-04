using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<int>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();

        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return Result.Failure<int>(UserErrors.EmailAlreadyTaken);
        }

        var newUser = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = _passwordHasher.Hash(request.Password),
        };
        
        userRepository.Add(newUser);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return newUser.Id;
    }
}