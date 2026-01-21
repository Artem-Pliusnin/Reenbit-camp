using Domain.Shared;

namespace Domain.Errors;

public static class PermissionErrors
{
    public static readonly Error InsufficientPermissions = new(
        "Permission.Forbidden",
        "You do not have permission to perform this action.");
}