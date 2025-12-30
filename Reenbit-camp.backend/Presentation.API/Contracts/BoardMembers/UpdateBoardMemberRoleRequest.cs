using System.Text.Json.Serialization;
using Domain.Enums;

namespace Presentation.API.Contracts.BoardMembers;

public sealed record UpdateBoardMemberRoleRequest(
    int BoardMemberId, 
    
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    BoardRole Role
    );