using Domain.Models.BoardMembers;
using Domain.Models.Comments;
using Domain.Requests.Comments;
using Microsoft.AspNetCore.Components;
using Services.Abstractions.Services;
using Telerik.Blazor.Components;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace WebApp.Components.Comments;

public partial class CommentRow : ComponentBase
{
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public CommentModel Comment { get; set; }
    
    [Parameter, EditorRequired] 
    public EventCallback<CommentModel> OnDelete { get; set; }
    
    [Inject]
    private ICommentsService CommentsService { get; set; } = default!;
    
    private bool IsEditing { get; set; }
    
    private string? InputComment = string.Empty;
    
    private TelerikPopover? PopoverRef { get; set; }

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
                .UpdateAsync(Comment.Id, new UpdateCommentRequest(Comment.Id, InputComment));

            if (result.IsSuccess)
            {
                Comment.Text = InputComment;
                Comment.IsEdited = true;
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
        var result = await CommentsService.DeleteAsync(Comment.Id);

        if (result.IsSuccess)
        {
            await OnDelete.InvokeAsync(Comment);
        }

        ClosePopover();
    }
}