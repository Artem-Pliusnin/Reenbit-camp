using Domain.Shared;

namespace Domain.Errors;

public static class SessionErrors
{
    public static readonly Error InvalidRefreshToken = new(
        "Session.InvalidRefreshToken",
        "Invalid or expired refresh token");
    
    public static readonly Error NotFound = new(
        "Session.NotFound",
        "User session doesn't exist.");
}