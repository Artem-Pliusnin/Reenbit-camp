using AutoMapper;
using Domain.DTOs.Lists;
using Domain.Entities;

namespace Application.Mapping;

public class ListProfile : Profile
{
    public ListProfile()
    {
        CreateMap<List, ListDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));
    }
}