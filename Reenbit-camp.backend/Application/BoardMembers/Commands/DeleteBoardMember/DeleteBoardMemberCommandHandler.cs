using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.BoardMembers;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.BoardMembers.Commands.DeleteBoardMember;

internal class DeleteBoardMemberCommandHandler : ICommandHandler<DeleteBoardMemberCommand, DeleteBoardMemberDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public DeleteBoardMemberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<DeleteBoardMemberDto>> Handle(
        DeleteBoardMemberCommand request, 
        CancellationToken cancellationToken)
    {
        var boardMemberRepository = _unitOfWork.GetRepository<IBoardMemberRepository>();
        
        var boardMember = await boardMemberRepository
            .GetByIdWithBoardAsync(request.BoardMemberId, cancellationToken);

        if (boardMember == null)
        {
            return Result.Failure<DeleteBoardMemberDto>(BoardMemberErrors.BoardMemberDoesNotExistError);
        }
        
        BoardMember? newOwner = null;
        if (boardMember.Role == BoardRole.Owner)
        {
            newOwner = await boardMemberRepository
                .GetNewOwnerAsync(boardMember.BoardId, boardMember ,cancellationToken);
        }
        
        boardMemberRepository.Remove(boardMember);

        if (newOwner != null)
        {
            newOwner.Role = BoardRole.Owner;
            boardMemberRepository.Update(newOwner);
        }
        
        var cardMembersRepository = _unitOfWork.GetRepository<ICardMembersRepository>();
        
        await cardMembersRepository
            .DeleteAllByUserAndBoard(
                boardMember.UserId, 
                boardMember.BoardId, 
                cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new DeleteBoardMemberDto
        {
            RemovedMemberId = boardMember.Id,
            NewOwner = _mapper.Map<BoardMemberDto>(newOwner),
        };
        
        return response;
    }
}