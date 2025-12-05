using Application.Abstractions.Messaging;
using Domain.DTOs.Boards;

namespace Application.Boards.Commands.CreateBoard;

public sealed record CreateBoardCommand(
    int UserId,
    string Title) 
    : ICommand<BoardDto>;