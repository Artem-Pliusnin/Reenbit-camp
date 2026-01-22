using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Users;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Queries.GetUserProfileInfo;

public class GetUserProfileInfoQueryHandler : IQueryHandler<GetUserProfileInfoQuery, UserProfileDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserProfileInfoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<UserProfileDto>> Handle(
        GetUserProfileInfoQuery request, 
        CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        
        var user = await userRepository
            .GetByIdWithAvatarAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure<UserProfileDto>(UserErrors.UserDoesNotExistError);
        }
        
        var response = _mapper.Map<UserProfileDto>(user);
        
        return response;
    }
}