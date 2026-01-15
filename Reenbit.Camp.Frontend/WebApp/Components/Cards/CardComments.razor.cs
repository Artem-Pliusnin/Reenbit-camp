using AutoMapper;
using Domain.Constants.HubConstants;
using Domain.DTOs.Cards;
using Domain.Models.BoardMembers;
using Domain.Models.Comments;
using Domain.Models.Users;
using Domain.Requests.Comments;
using Domain.Responses.Cards;
using Domain.Responses.Comments;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Services.Abstractions.Services;
using Services.HubServices;

namespace WebApp.Components.Cards;

public partial class CardComments : ComponentBase, IDisposable
{
    [CascadingParameter(Name="CurrentUser")]
    public BoardMemberModel CurrentUser { get; set; } = default!;
    
    [Parameter, EditorRequired]
    public int CardId { get; set; }
    
    [Inject]
    private ICommentsService CommentsService { get; set; } = default!;
    
    [Inject]
    private IMapper Mapper { get; set; } = default!;
    
    [Inject] 
    public HubConnectionManager HubConnectionManager { get; set; } = default!;
    
    private HubConnection TaskHubConnection;

    private List<IDisposable> Subscriptions = new();
    
    private bool isLoading;
    
    private List<CommentModel> Comments = new();
    
    private bool IsWritingComment;
    
    private string? InputComment = string.Empty;
    
    protected override async Task OnInitializedAsync()
    {
        TaskHubConnection = HubConnectionManager.Get(HubType.TaskHub);
        
        var commentsResult = await CommentsService
            .GetByCardAsync(CardId);

        if (commentsResult.IsSuccess)
        {
            Comments = commentsResult.Value;
        }
        
        Subscriptions.Add(TaskHubConnection
            .On<CommentDto>(
                SubscribeTaskHubConstants.AddComment, 
                AddComment));
        
        Subscriptions.Add(TaskHubConnection
            .On<int>(
                SubscribeTaskHubConstants.DeleteComment, 
                RemoveComment));
        
        Subscriptions.Add(TaskHubConnection
            .On<CommentDto>(
                SubscribeTaskHubConstants.UpdateComment, 
                UpdateComment));
    }
    
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
                
                var dto = Mapper.Map<CommentDto>(result.Value);
                await TaskHubConnection
                    .SendAsync(
                        SendTaskHubConstants.AddComment, 
                        dto,
                        CardId);
            }
        }
        
        InputComment = string.Empty;
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

    private async Task DeleteComment(CommentModel comment)
    {
        Comments.Remove(comment);
        await TaskHubConnection
            .SendAsync(
                SendTaskHubConstants.DeleteComment,
                comment.Id,
                CardId);
    }

    private void AddComment(CommentDto comment)
    {
        if (!Comments.Any(c => c.Id == comment.Id))
        {
            var commentModel = Mapper.Map<CommentModel>(comment);
            Comments.Insert(0,commentModel);
            
            StateHasChanged();
        }
    }
    
    private void UpdateComment(CommentDto comment)
    {
        var updatedComment = Comments.FirstOrDefault(l => l.Id == comment.Id);
        
        if (updatedComment is not null)
        {
            updatedComment.Text = comment.Text;
            updatedComment.IsEdited = comment.IsEdited;
            
            StateHasChanged();
        }
    }

    private void RemoveComment(int commentId)
    {
        Comments.RemoveAll(c => c.Id == commentId);
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