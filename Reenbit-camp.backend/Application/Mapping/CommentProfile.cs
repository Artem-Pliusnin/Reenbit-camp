using AutoMapper;
using Domain.DTOs.Comments;
using Domain.Entities;

namespace Application.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>();
    }
}