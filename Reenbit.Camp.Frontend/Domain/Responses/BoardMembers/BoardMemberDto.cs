using Domain.Enums;
using Domain.Responses.Users;

namespace Domain.Responses.BoardMembers;

public sealed record BoardMemberDto(
    int Id, 
    int BoardId, 
    UserDto User, 
    BoardRole Role);
