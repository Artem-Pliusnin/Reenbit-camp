using Application.Abstractions.Messaging;

namespace Application.FAQChat.Queries.AskQuestion;

public sealed record AskQuestionQuery(int UserId, string Question) : IQuery<string>;