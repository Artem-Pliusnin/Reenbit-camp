namespace Domain.Requests.Users;

public record UpdateUserPasswordRequest(        
    string Password,
    string NewPassword
);