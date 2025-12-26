using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Users;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Queries.GetInviteSuggestionUsers;

internal class GetInviteSuggestionUsersQueryHandler 
    : IQueryHandler<GetInviteSuggestionUsersQuery, List<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetInviteSuggestionUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<List<UserDto>>> Handle(GetInviteSuggestionUsersQuery request, CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
        
        var users = await userRepository.GetForInvitationAsync(
            request.BoardId, 
            request.UserId, 
            request.Query, 
            request.Limit, 
            cancellationToken);
        
        var response = _mapper.Map<List<UserDto>>(users);
        
        return response;
    }
}