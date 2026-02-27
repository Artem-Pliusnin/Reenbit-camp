using Domain.Responses.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Services.Authentication;

namespace WebApp.Pages;

public partial class AuthCallbackPage : ComponentBase
{
    [SupplyParameterFromQuery] 
    public string? AccessToken { get; set; }
    
    [SupplyParameterFromQuery] 
    public string? RefreshToken { get; set; }
    
    [SupplyParameterFromQuery] 
    public string? AccessTokenExpiresAt { get; set; }
    
    [SupplyParameterFromQuery] 
    public string? RefreshTokenExpiresAt { get; set; }
    
    [Inject] 
    public NavigationManager Navigation{ get; set; } = default!;
    
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine(1111);
        if (string.IsNullOrEmpty(AccessToken) || string.IsNullOrEmpty(RefreshToken))
        {
            Navigation.NavigateTo("/error", replace: true);
            return;
        }

        Console.WriteLine(AccessToken);
        Console.WriteLine(RefreshToken);
        Console.WriteLine(AccessTokenExpiresAt);
        Console.WriteLine(RefreshTokenExpiresAt);

        var session = new TokensResponseDto
        (
            AccessToken,
            RefreshToken,
            DateTime.Parse(AccessTokenExpiresAt!, null, 
                System.Globalization.DateTimeStyles.RoundtripKind),
            DateTime.Parse(RefreshTokenExpiresAt!, null, 
                System.Globalization.DateTimeStyles.RoundtripKind)
        );

        await ((JwtAuthStateProvider)AuthenticationStateProvider)
            .MarkUserAsLoggedInAsync(session);

        Navigation.NavigateTo("/");
    }
}