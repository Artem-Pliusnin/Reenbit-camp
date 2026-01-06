using Application.Abstractions.Messaging;
using Domain.DTOs.Comments;

namespace Application.Comments.Queries.GetCardComments;

public sealed record GetCardCommentsQuery(int CardId) : IQuery<List<CommentDto>>;