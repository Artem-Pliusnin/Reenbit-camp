using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.UpdateUserInfo;

internal class UpdateUserInfoCommandHandler : ICommandHandler<UpdateUserInfoCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserInfoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Result.Failure(UserErrors.UserDoesNotExistError);
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(ListErrors.UpdateListError);
        }
    }
}