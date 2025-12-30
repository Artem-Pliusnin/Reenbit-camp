using System.Security.Claims;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.API.Abstractions;

[Authorize]
public class AuthorizedContoller : ApiController
{
    public AuthorizedContoller(ISender sender)
        : base(sender)
    {}

    protected bool TryGetUserId(out int userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim.Value, out userId))
        {
            userId = 0;
            return false;
        }
        
        return true;
    }
}