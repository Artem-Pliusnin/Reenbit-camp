using Application.Abstractions.Messaging;
using Domain.DTOs.Users;

namespace Application.Users.Queries.GetUserInfo;

public sealed record GetUserInfoQuery(int UserId) : IQuery<UserDto>;