using Application.Abstractions.Messaging;

namespace Application.Comments.Commands.DeleteComment;

public sealed record DeleteCommentCommand(int CommentId, int UserId) : ICommand;