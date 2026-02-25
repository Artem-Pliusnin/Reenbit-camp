using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.Models.BoardMembers;
using Domain.Models.Boards;
using Domain.Models.Comments;
using Domain.Requests.Comments;
using Domain.Responses.Comments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;
using Telerik.Blazor.Components;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace WebApp.Components.Comments;

public partial class CommentRow : ComponentBase
{
    [CascadingParameter(Name="Board")]
    public BoardInfoModel Board { get; set; } = default!;
    
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public int CardId { get; set; }
    
    [Parameter, EditorRequired]
    public CommentModel Comment { get; set; }
    
    [Parameter, EditorRequired] 
    public EventCallback<CommentModel> OnDelete { get; set; }
    
    [Inject]
    private ICommentsService CommentsService { get; set; } = default!;
    
    [Inject]
    private IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection TaskHubConnection;
    
    private bool IsEditing { get; set; }
    
    private string? InputComment = string.Empty;
    
    private TelerikPopover? PopoverRef { get; set; }

    protected override async Task OnInitializedAsync()
    {
        TaskHubConnection = HubConnectionManager.Get(HubType.TaskHub);
    }

    private void EditComment()
    {
        IsEditing = true;
        InputComment = Comment.Text;
    }

    private async Task SaveComment()
    {
        if (!string.IsNullOrWhiteSpace(InputComment))
        {
            var result = await CommentsService
                .UpdateAsync(
                    Board.Id, 
                    Comment.Id, 
                    new UpdateCommentRequest(Comment.Id, InputComment));

            if (result.IsSuccess)
            {
                Comment.Text = InputComment;
                Comment.IsEdited = true;
                
                var dto = Mapper.Map<CommentDto>(Comment);
                await TaskHubConnection
                    .SendAsync(
                        SendTaskHubConstants.UpdateComment, 
                        dto,
                        CardId);
            }
        }
        else
        {
            await DeleteComment();
        }
        
        IsEditing = false;
        InputComment = string.Empty;
    }

    private void CancelWritingComment()
    {
        IsEditing = false;
        InputComment = string.Empty;
    }
    
    private void OpenPopover()
    {
        PopoverRef?.Show();
    }
    
    private void ClosePopover()
    {
        PopoverRef?.Hide();
    }

    private async Task DeleteComment()
    {
        var result = await CommentsService.DeleteAsync(Board.Id, Comment.Id);

        if (result.IsSuccess)
        {
            await OnDelete.InvokeAsync(Comment);
        }

        ClosePopover();
    }
}