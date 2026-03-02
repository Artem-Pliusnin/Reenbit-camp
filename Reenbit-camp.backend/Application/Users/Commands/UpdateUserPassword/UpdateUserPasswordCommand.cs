using Application.Abstractions.Messaging;

namespace Application.Users.Commands.UpdateUserPassword;

public sealed record UpdateUserPasswordCommand(
    int UserId,
    string Password, 
    string NewPassword) 
    : ICommand;