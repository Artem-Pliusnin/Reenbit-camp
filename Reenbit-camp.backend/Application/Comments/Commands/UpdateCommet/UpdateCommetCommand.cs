using Application.Abstractions.Messaging;

namespace Application.Comments.Commands.UpdateCommet;

public sealed record UpdateCommetCommand(
    int CommentId, 
    int UserId, 
    string Text) : ICommand;