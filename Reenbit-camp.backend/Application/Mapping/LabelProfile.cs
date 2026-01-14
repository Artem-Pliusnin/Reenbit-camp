using AutoMapper;
using Domain.DTOs.Labels;
using Domain.Entities;

namespace Application.Mapping;

public class LabelProfile: Profile
{
    public LabelProfile()
    {
        CreateMap<Label, LabelDto>();
        
        CreateMap<Label, UpdatedLabelDto>()
            .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src));
    }
}