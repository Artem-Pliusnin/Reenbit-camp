using Application.Abstractions.Messaging;

namespace Application.Users.Commands.UpdateUserInfo;

public sealed record UpdateUserInfoCommand(
    int UserId, 
    string FirstName, 
    string LastName) : ICommand;