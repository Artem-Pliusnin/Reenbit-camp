using Domain.Enums;

namespace Domain.Extensions;

public static class BoardRoleExtensions
{
    public static bool HasAtLeast(
        this BoardRole role, 
        BoardRole required) => role <= required;
}