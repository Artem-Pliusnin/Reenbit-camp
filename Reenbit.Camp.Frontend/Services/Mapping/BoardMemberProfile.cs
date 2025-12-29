using AutoMapper;
using Domain.Models.BoardMembers;
using Domain.Responses.BoardMembers;

namespace Services.Mapping;

public class BoardMemberProfile : Profile
{
    public BoardMemberProfile()
    {
        CreateMap<BoardMemberDto, BoardMemberModel>();
    }
}