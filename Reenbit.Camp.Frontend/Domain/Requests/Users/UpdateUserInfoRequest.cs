namespace Domain.Requests.Users;

public sealed record UpdateUserInfoRequest(
    string FirstName, 
    string LastName);