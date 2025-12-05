using Domain.Shared;

namespace Domain.Errors;

public static class SessionErrors
{
    public static readonly Error InvalidRefreshToken = new(
        "Session.InvalidRefreshToken",
        "Invalid or expired refresh token");
    
    public static readonly Error RefreshTokenUserMismatch = new(
        "Session.RefreshTokenUserMismatch",
        "The refresh token does not belong to the specified user");
}