using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Auth.Commands.RegisterUser;

internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, int>
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
        var subcriptionPlansRepository = _unitOfWork.GetRepository<ISubscriptionPlansRepository>();
        var usersSubcriptionsRepository = _unitOfWork.GetRepository<IUserSubscriptionsRepository>();

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
        
        var freePlan = await subcriptionPlansRepository
            .GetBySubscriptionName("Free", cancellationToken);
        
        var userSubscription = new UserSubscription {
            UserId = newUser.Id,
            SubscriptionPlanId = freePlan?.Id ?? 1,
            SubscriptionStatus = SubscriptionStatus.Active,
            CurrentPeriodEnd = null
        };
        
        usersSubcriptionsRepository.Add(userSubscription);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return newUser.Id;
    }
}