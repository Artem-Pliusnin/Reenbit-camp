using AutoMapper;
using Domain.DTOs.Boards;
using Domain.Models.Boards;
using Domain.Responses.Boards;

namespace Services.Mapping;

public class BoardProfile : Profile
{
    public BoardProfile()
    {
        CreateMap<BoardDto, BoardModel>().ReverseMap();
        
        CreateMap<BoardInfoDto, BoardInfoModel>();
        
        CreateMap<BoardCardDto, BoardCardModel>();
    }
}