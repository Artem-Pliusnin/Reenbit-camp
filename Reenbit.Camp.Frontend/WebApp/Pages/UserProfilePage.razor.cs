using Domain.Constants.FileConstants;
using Domain.Models.Users;
using Domain.Requests.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Refit;
using Services.Abstractions.Services;
using Telerik.Blazor.Components;

namespace WebApp.Pages;

public partial class UserProfilePage : ComponentBase
{
    [Parameter]
    public int UserId { get; set; }
    
    [Inject] 
    public IUsersService UsersService { get; set; } = default!;
    
    private UserProfileModel userProfile;
    
    private string enteredFirstName = string.Empty;
    
    private string enteredLastName = string.Empty;
    
    private bool isPageLoading;
    
    TelerikDialog DialogRef;
    
    private bool IsAvatarWindowOpen;

    private bool isFileLoading;
    
    private IBrowserFile? selectedFile;
    
    private string? previewImageUrl;

    protected override async Task OnParametersSetAsync()
    {
        isPageLoading = true;
        
        var result = await UsersService.GetUserProfileAsync(UserId);

        if (result.IsSuccess)
        {
            userProfile = result.Value;
            
            enteredFirstName = userProfile.FirstName;
            enteredLastName = userProfile.LastName;
            
            isPageLoading = false;
        }
    }

    private void OpenAvatarWindow()
    {
        IsAvatarWindowOpen = true;
        selectedFile = null;
        if (userProfile.Avatar != null)
        {
            previewImageUrl = userProfile.Avatar.FileUrl;
        }
    }
    
    private void CloseAvatarWindow()
    {
        IsAvatarWindowOpen = false;
        selectedFile = null;
        previewImageUrl = null;
    }
    
    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        isFileLoading = true;
        DialogRef.Refresh();
        
        var start = DateTime.Now;
        
        selectedFile = e.File;
        
        if (selectedFile != null)
        {
            var stream = selectedFile.OpenReadStream(FilesConstants.MaxFileSize);
            
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer);
            
            previewImageUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
        }

        Console.WriteLine((DateTime.Now - start).TotalMilliseconds);
        
        isFileLoading = false;
        DialogRef.Refresh();
    }
    
    private async Task SaveAvatar()
    {
        if (selectedFile == null)
        {
            return;
        }
        
        var stream = selectedFile.OpenReadStream(FilesConstants.MaxFileSize);
        
        var file = new StreamPart(stream, selectedFile.Name, selectedFile.ContentType);
        
        var result = await UsersService.UpdateUserAvatarAsync(file, UserId);

        if (result.IsSuccess)
        {
            userProfile.Avatar = result.Value;
        }

        CloseAvatarWindow();
    }

    private bool CanSaveProfile()
    {
        return (userProfile.FirstName != enteredFirstName 
               || userProfile.LastName != enteredLastName) 
               && !string.IsNullOrWhiteSpace(enteredFirstName)
               && !string.IsNullOrWhiteSpace(enteredLastName); ;
    }
    
    private async Task SaveName()
    {
        if (string.IsNullOrEmpty(enteredFirstName) || string.IsNullOrEmpty(enteredLastName))
        {
            return;
        }
        
        var result = await UsersService.UpdateUserInfoAsync(
            new UpdateUserInfoRequest(
                enteredFirstName,
                enteredLastName),
            UserId);

        if (result.IsSuccess)
        {
            userProfile.FirstName = enteredFirstName;
            userProfile.LastName = enteredLastName;
        }
        
        StateHasChanged();
    }
}