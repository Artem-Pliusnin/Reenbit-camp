using Application.Abstractions.Messaging;
using Domain.DTOs.UserAvatars;

namespace Application.Users.Commands.UpdateUserAvatar;

public sealed record UpdateUserAvatarCommand(
    int UserId,
    Stream FileContent,
    string FileName,
    string ContentType) : ICommand<UserAvatarDto>;