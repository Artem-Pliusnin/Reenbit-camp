using AutoMapper;
using Domain.DTOs.Boards;
using Domain.Entities;

namespace Application.Mapping;

public class BoardProfile : Profile
{
    public BoardProfile()
    {
        CreateMap<Board, BoardDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));
    }
}