using AutoMapper;
using Domain.DTOs.Boards;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mapping;

public class BoardProfile : Profile
{
    public BoardProfile()
    {
        CreateMap<Board, BoardDto>();

        CreateMap<Board, BoardInfoDto>();
        
        CreateMap<Board, BoardCardDto>()
            .ForMember(
                dest => dest.OwnerId,
                opt => opt.MapFrom(src =>
                    src.Members
                        .Where(x => x.Role == BoardRole.Owner)
                        .Select(x => (int?)x.UserId)
                        .FirstOrDefault()
                    ?? src.CreatedBy
                )
            );
    }
}