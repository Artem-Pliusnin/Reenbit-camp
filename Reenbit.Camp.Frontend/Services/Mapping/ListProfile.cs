using AutoMapper;
using Domain.Models.Lists;
using Domain.Responses.Lists;

namespace Services.Mapping;

public class ListProfile : Profile
{
    public ListProfile()
    {
        CreateMap<ListDto, ListModel>();
    }
}