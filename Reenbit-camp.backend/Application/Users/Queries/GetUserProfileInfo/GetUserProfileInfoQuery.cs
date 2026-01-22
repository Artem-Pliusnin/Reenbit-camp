using Application.Abstractions.Messaging;
using Domain.DTOs.Users;

namespace Application.Users.Queries.GetUserProfileInfo;

public sealed record GetUserProfileInfoQuery(int UserId) : IQuery<UserProfileDto>;