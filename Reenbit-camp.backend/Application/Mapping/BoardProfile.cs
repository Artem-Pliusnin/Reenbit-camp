using AutoMapper;
using Domain.DTOs.Boards;
using Domain.Entities;

namespace Application.Mapping;

public class BoardProfile : Profile
{
    public BoardProfile()
    {
        CreateMap<Board, BoardDto>();

        CreateMap<Board, BoardInfoDto>();
    }
}