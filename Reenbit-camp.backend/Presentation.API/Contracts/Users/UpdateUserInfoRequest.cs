namespace Presentation.API.Contracts.Users;

public sealed record UpdateUserInfoRequest(
    string FirstName, 
    string LastName);