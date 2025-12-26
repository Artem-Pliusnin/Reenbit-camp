using AutoMapper;
using Domain.DTOs.BoardMembers;
using Domain.Entities;

namespace Application.Mapping;

public class BoardMemberProfile : Profile
{
    public BoardMemberProfile()
    {
        CreateMap<BoardMember, BoardMemberDto>();
    }
}