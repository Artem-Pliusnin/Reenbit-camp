using Application.Abstractions.Messaging;
using Domain.DTOs.Comments;

namespace Application.Comments.Commands.CreateComment;

public sealed record CreateCommentCommand(
    int CardId, 
    int UserId, 
    string Text) : ICommand<CommentDto>;