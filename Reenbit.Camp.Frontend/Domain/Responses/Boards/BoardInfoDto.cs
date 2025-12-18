using Domain.Responses.Lists;

namespace Domain.Responses.Boards;

public sealed record BoardInfoDto(
    int Id, 
    string Title, 
    List<ListDto> Lists);