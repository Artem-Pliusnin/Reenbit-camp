using Domain.Responses.Boards;
using Domain.Responses.Users;

namespace Domain.Responses.Invitations;

public sealed record InvitationDto(
    int Id,
    BoardDto Board,
    UserDto InvitedUser,
    UserDto InvitedByUser,
    DateTime SentDate);