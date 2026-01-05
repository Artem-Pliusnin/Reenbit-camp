using Domain.Responses.Users;

namespace Domain.Responses.CardMembers;

public sealed record CardMemberDto(
    int Id, 
    UserDto User);