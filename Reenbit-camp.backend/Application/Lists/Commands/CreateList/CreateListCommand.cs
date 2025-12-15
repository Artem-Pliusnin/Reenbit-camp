using Application.Abstractions.Messaging;
using Domain.DTOs.Lists;

namespace Application.Lists.Commands.CreateList;

public sealed record CreateListCommand(
    int BoardId,
    string Title,
    int UserId) 
    : ICommand<ListDto>;