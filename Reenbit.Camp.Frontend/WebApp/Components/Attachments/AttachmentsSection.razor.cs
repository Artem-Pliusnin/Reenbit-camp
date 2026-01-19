using AutoMapper;
using Domain.Constants.FileConstants;
using Domain.Constants.HubConstants;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models.BoardMembers;
using Domain.Models.CardAttachments;
using Domain.Models.Labels;
using Domain.Responses.CardAttachments;
using Domain.Responses.Comments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Refit;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;

namespace WebApp.Components.Attachments;

public partial class AttachmentsSection : ComponentBase, IDisposable
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
        
        Subscriptions.Add(TaskHubConnection
            .On<CardAttachmentDto>(
                SubscribeTaskHubConstants.AddAttachment, 
                AddAttachment));
        
        Subscriptions.Add(TaskHubConnection
            .On<int>(
                SubscribeTaskHubConstants.DeleteAttachment, 
                RemoveAttachment));
    }
    
    private bool CheckCardAttachmentsManagingPermision() => CurrentUser.Role.HasAtLeast(BoardRole.Member);

    private void OpenAddMenu()
    {
        if (CheckCardAttachmentsManagingPermision())
        {
            SelectedFile = null;
            PopoverRef?.Show();
        }
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
            
            var dto = Mapper.Map<CardAttachmentDto>(result.Value);
            await TaskHubConnection
                .SendAsync(
                    SendTaskHubConstants.AddAttachment, 
                    dto,
                    CardId);
        }
        
        CloseAddMenu();
    }
    
    private async Task DeleteAttachment(CardAttachmentModel attachment)
    {
        var result = await CardAttachmentService.DeleteAsync(attachment.Id);

        if (result.IsSuccess)
        {
            Attachments.Remove(attachment);
            await TaskHubConnection
                .SendAsync(
                    SendTaskHubConstants.DeleteAttachment,
                    attachment.Id,
                    CardId);
        }
    }

    bool IsImage(CardAttachmentModel attachment)
        => attachment.ContentType.StartsWith("image/");

    private void AddAttachment(CardAttachmentDto attachment)
    {
        if (!Attachments.Any(ca => ca.Id == attachment.Id))
        {
            var attachmentModel = Mapper.Map<CardAttachmentModel>(attachment);
            Attachments.Insert(0, attachmentModel);
            
            StateHasChanged();
        }
    }
    
    private void RemoveAttachment(int attachmentId)
    {
        Attachments.RemoveAll(ca => ca.Id == attachmentId);
        StateHasChanged();
    }

    public void Dispose()
    {
        foreach (var subscription in Subscriptions)
        {
            subscription.Dispose();
        }
    }
}