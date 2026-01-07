using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Comments;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Comments.Commands.CreateComment;

internal class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, CommentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCommentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<CommentDto>> Handle(
        CreateCommentCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            
            var user = await userRepository
                .GetByIdAsync(request.UserId, cancellationToken);
            
            if (user == null)
            {
                return Result.Failure<CommentDto>(UserErrors.UserDoesNotExistError);
            }
            
            var commentsRepository = _unitOfWork.GetRepository<ICommentsRepository>();

            var comment = new Comment()
            {
                CardId = request.CardId,
                Text = request.Text,
                User = user,
                CreatedAt = DateTime.UtcNow,
            };
            
            commentsRepository.Add(comment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CommentDto>(comment);

            return response;
        }
        catch
        {
            return Result.Failure<CommentDto>(CommentErrors.CreateCommentError);
        }
    }
}