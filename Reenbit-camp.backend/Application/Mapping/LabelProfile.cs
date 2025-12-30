using AutoMapper;
using Domain.DTOs.Labels;
using Domain.Entities;

namespace Application.Mapping;

public class LabelProfile: Profile
{
    public LabelProfile()
    {
        CreateMap<Label, LabelDto>();
    }
}