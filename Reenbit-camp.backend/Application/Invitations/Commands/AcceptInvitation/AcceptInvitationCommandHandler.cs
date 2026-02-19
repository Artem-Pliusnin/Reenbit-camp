using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.BoardMembers;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Invitations.Commands.AcceptInvitation;

internal class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand, BoardMemberDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AcceptInvitationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<BoardMemberDto>> Handle(
        AcceptInvitationCommand request, 
        CancellationToken cancellationToken)
    {
        var invitationRepository = _unitOfWork.GetRepository<IInvitationRepository>();
        
        var invitation = await invitationRepository.GetByIdAsync(request.InvitationId);
        
        if (invitation == null)
        {
            return Result.Failure<BoardMemberDto>(InvitationErrors.InvitationDoesNotExist);
        }

        if (invitation.InvitedUserId != request.UserId)
        {
            return Result.Failure<BoardMemberDto>(InvitationErrors.InvitationDoesNotBelongsToUserError);
        }
        
        var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            
        var user = await userRepository.GetByIdWithAvatarAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            return Result.Failure<BoardMemberDto>(UserErrors.UserDoesNotExistError);
        }
        
        try
        {
            invitation.Status = InvitationStatus.Accepted;
            invitation.RespondedAt = DateTime.UtcNow;
            
            invitationRepository.Update(invitation);
            
            var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();

            var boardMember = new BoardMember()
            {
                BoardId = invitation.BoardId,
                User = user,
                Role = BoardRole.Member
            };

            boardMemberRepository.Add(boardMember);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var response = _mapper.Map<BoardMemberDto>(boardMember);

            return response;
        }
        catch
        {
            return Result.Failure<BoardMemberDto>(InvitationErrors.UpdateInvitationError);
        }
    }
}