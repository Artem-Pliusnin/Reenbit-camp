using Application.Abstractions.Messaging;

namespace Application.Auth.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName, 
    string Email,
    string Password)
    : ICommand<int>;
