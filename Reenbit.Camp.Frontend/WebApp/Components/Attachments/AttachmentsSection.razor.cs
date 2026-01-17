using AutoMapper;
using Domain.Constants.FileConstants;
using Domain.Models.BoardMembers;
using Domain.Models.CardAttachments;
using Domain.Models.Labels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Refit;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;

namespace WebApp.Components.Attachments;

public partial class AttachmentsSection : ComponentBase
{
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;
    
    [Parameter, EditorRequired] 
    public int CardId { get; set; }
    
    [Inject] 
    private ICardAttachmentService CardAttachmentService { get; set; } = default!;
    
    [Inject] 
    private IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection TaskHubConnection;
    
    private List<IDisposable> Subscriptions = new();
    
    private List<CardAttachmentModel> Attachments { get; set; } = new();
    
    TelerikPopover? PopoverRef;
    
    IBrowserFile? SelectedFile;

    protected override async Task OnInitializedAsync()
    {
        TaskHubConnection = HubConnectionManager.Get(HubType.TaskHub);
        
        var result = await CardAttachmentService
            .GetByCardAsync(CardId);

        if (result.IsSuccess)
        {
            Attachments = result.Value;
        }
    }

    private void OpenAddMenu()
    {
        SelectedFile = null;
        PopoverRef?.Show();
    }

    private void CloseAddMenu()
    {
        PopoverRef?.Hide();
    }

    private void OnFileSelected(InputFileChangeEventArgs e)
    { 
        SelectedFile = e.File;
        
        PopoverRef?.Refresh();
    }

    private async Task Upload()
    {
        if (SelectedFile == null)
        {
            return;
        }
        
        var stream = SelectedFile.OpenReadStream(FilesConstants.MaxFileSize);
        
        var file = new StreamPart(stream, SelectedFile.Name, SelectedFile.ContentType);
        
        var result = await CardAttachmentService
            .CreateAsync(CardId, file);

        if (result.IsSuccess)
        {
            Attachments.Insert(0, result.Value);
        }
        
        CloseAddMenu();
    }
    
    private async Task DeleteAttachment(CardAttachmentModel attachment)
    {
        var result = await CardAttachmentService.DeleteAsync(attachment.Id);

        if (result.IsSuccess)
        {
            Attachments.Remove(attachment);
        }
    }

    bool IsImage(CardAttachmentModel attachment)
        => attachment.ContentType.StartsWith("image/");

}