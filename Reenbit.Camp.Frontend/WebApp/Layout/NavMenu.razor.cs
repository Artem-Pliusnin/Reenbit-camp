using Domain.Models.Users;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;

namespace WebApp.Layout;

public partial class NavMenu
{
    [Parameter, EditorRequired] 
    public int UserId { get; set; }
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    
    [Inject]
    public IUsersService UsersService { get; set; } = default!;
    
    private UserModel CurrentUser;

    private bool isLoadinUser;

    private bool isOpen;

    protected override async Task OnInitializedAsync()
    {
        isLoadinUser = true;
        
        var result = await UsersService.GetUserInfoAsync(UserId);

        if (result.IsSuccess)
        { 
            CurrentUser = result.Value;
            isLoadinUser = false;
        }
    }

    private void ToggleMenu()
    {
        isOpen = !isOpen;
    }

    private void NavigateToProfile()
    {
        NavigationManager.NavigateTo($"/userProfile/{UserId}");
    }
    
}