using System.Reflection.Emit;
using AutoMapper;
using Domain.Models.Labels;
using Domain.Responses.Labels;

namespace Services.Mapping;

public class LabelProfile : Profile
{
    public LabelProfile()
    {
        CreateMap<LabelDto, LabelModel>();
    }
}