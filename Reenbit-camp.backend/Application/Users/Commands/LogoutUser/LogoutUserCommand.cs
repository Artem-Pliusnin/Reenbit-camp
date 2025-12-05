using Application.Abstractions.Messaging;

namespace Application.Users.Commands.LogoutUser;

public sealed record LogoutUserCommand(
    int UserId, 
    string RefreshToken) 
    : ICommand<bool>;
