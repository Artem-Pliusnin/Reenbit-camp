using AutoMapper;
using Domain.Models.Comments;
using Domain.Responses.Comments;

namespace Services.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentDto, CommentModel>();
    }
}