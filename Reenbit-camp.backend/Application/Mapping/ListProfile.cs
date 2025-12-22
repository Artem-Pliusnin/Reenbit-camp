using AutoMapper;
using Domain.DTOs.Lists;
using Domain.Entities;

namespace Application.Mapping;

public class ListProfile : Profile
{
    public ListProfile()
    {
        CreateMap<List, ListDto>();
    }
}