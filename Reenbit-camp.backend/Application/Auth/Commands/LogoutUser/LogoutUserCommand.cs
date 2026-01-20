using Application.Abstractions.Messaging;

namespace Application.Auth.Commands.LogoutUser;

public sealed record LogoutUserCommand(
    int UserId) 
    : ICommand<bool>;
