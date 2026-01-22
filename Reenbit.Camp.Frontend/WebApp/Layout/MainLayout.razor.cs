using System.Security.Claims;

namespace WebApp.Layout;

public partial class MainLayout
{
    private NavMenu? navMenuRef;
    
    private int GetUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        return int.Parse(userIdClaim!.Value);
    }
}