using AutoMapper;
using Domain.Constants.FileConstants;
using Domain.Models.Users;
using Domain.Requests.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Refit;
using Services.Abstractions.Services;
using Telerik.Blazor.Components;
using WebApp.Layout;

namespace WebApp.Pages;

public partial class UserProfilePage : ComponentBase
{
    [Parameter]
    public int UserId { get; set; }
    
    [CascadingParameter(Name="NavMenu")]
    public NavMenu? NavMenuRef { get; set; }
    
    [Inject] 
    public IUsersService UsersService { get; set; } = default!;
    
    [Inject] 
    public IMapper Mapper { get; set; } = default!;
    
    private UserProfileModel userProfile;
    
    private string enteredFirstName = string.Empty;
    
    private string enteredLastName = string.Empty;
    
    private bool isPageLoading;
    
    TelerikDialog DialogRef;
    
    private bool IsAvatarWindowOpen;

    private bool isFileLoading;
    
    private IBrowserFile? selectedFile;
    
    private string? previewImageUrl;
    
    private string CurrentPassword = string.Empty;
    
    private string NewPassword = string.Empty;
    
    private bool isCurrentPasswordInvalid;

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
        
        selectedFile = e.File;
        
        if (selectedFile != null)
        {
            var stream = selectedFile.OpenReadStream(FilesConstants.MaxFileSize);
            
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer);
            
            previewImageUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
        }

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
            
            var userModel = Mapper.Map<UserModel>(userProfile);
            NavMenuRef!.UpdateProfile(userModel);
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
    
    private bool CanChangePassword()
    {
        return (NewPassword.Length >= 8 && NewPassword.Length <= 255)
               && (!userProfile.HasPassword ||
                   (CurrentPassword.Length >= 8 && CurrentPassword.Length <= 255));
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
            
            var userModel = Mapper.Map<UserModel>(userProfile);
            NavMenuRef!.UpdateProfile(userModel);
        }
        
        StateHasChanged();
    }
    
    private async Task ChangePassword()
    {
        if (!CanChangePassword())
        {
            return;
        }
        
        var result = await UsersService.UpdateUserPasswordAsync(
            new UpdateUserPasswordRequest(
                CurrentPassword,
                NewPassword),
            UserId);

        if (result.IsFailure)
        {
            isCurrentPasswordInvalid = true;
            return;
        }
        
        NewPassword = string.Empty;
        CurrentPassword = string.Empty;
        
        StateHasChanged();
    }
}