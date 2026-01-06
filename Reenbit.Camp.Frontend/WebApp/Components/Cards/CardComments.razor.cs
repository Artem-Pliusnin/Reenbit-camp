using Domain.Models.BoardMembers;
using Domain.Models.Comments;
using Domain.Models.Users;
using Domain.Requests.Comments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Services.Abstractions.Services;

namespace WebApp.Components.Cards;

public partial class CardComments : ComponentBase
{
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public int CardId { get; set; }
    
    [Inject]
    private ICommentsService CommentsService { get; set; } = default!;
    
    private bool isLoading;
    
    private List<CommentModel> Comments = new();
    
    private bool IsWritingComment;
    
    private string? InputComment = string.Empty;
    
    private void StartWritingComment()
    {
        IsWritingComment = true;
    }

    private async Task SaveComment()
    {
        if (!string.IsNullOrWhiteSpace(InputComment))
        {
            var result = await CommentsService
                .CreateAsync(new CreateCommentRequest(CardId, InputComment));

            if (result.IsSuccess)
            {
                Comments.Insert(0,result.Value);
            }
        }
        
        InputComment =string.Empty;
        IsWritingComment = false;
    }

    private void CancelWritingComment()
    {
        InputComment =string.Empty;
        IsWritingComment = false;
    }

    private async Task OnTitleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            SaveComment();
        }
        else if (e.Key == "Escape")
        {
            CancelWritingComment();
        }
    }

    private void DeleteComment(CommentModel comment)
    {
        Comments.Remove(comment);
    }
    
    protected override async Task OnInitializedAsync()
    {
        var commentsResult = await CommentsService
            .GetByCardAsync(CardId);

        if (commentsResult.IsSuccess)
        {
            Comments = commentsResult.Value;
        }
    }
}