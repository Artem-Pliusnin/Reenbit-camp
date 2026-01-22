using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Users;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Queries.GetUserInfo;

internal class GetUserInfoQueryHandler : IQueryHandler<GetUserInfoQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserInfoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<UserDto>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        
        var user = await userRepository
            .GetByIdWithAvatarAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure<UserDto>(UserErrors.UserDoesNotExistError);
        }
        
        var response = _mapper.Map<UserDto>(user);
        
        return response;
    }
}