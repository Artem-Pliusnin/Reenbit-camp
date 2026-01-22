using AutoMapper;
using Domain.Models.BoardMembers;
using Domain.Requests.BoardMembers;
using Domain.Responses.BoardMembers;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class BoardMembersService : IBoardMembersService
{
    private readonly IBoardMembersApi _boardMembersApi;
    private readonly IMapper _mapper;

    public BoardMembersService(IBoardMembersApi boardMembersApi, IMapper mapper)
    {
        _boardMembersApi = boardMembersApi;
        _mapper = mapper;
    }
    
    public async Task<Result<List<BoardMemberModel>>> GetByBoardAsync(int boardId)
    {
        var response = await _boardMembersApi.GetByBoardAsync(boardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<BoardMemberModel>>(content));
    }

    public async Task<Result<BoardMemberModel>> GetCurrentAsync(int boardId)
    {
        var response = await _boardMembersApi.GetCurrentAsync(boardId);

        return response.HandleResultWithMapping(content 
            => _mapper.Map<BoardMemberModel>(content));
    }

    public async Task<Result<object>> UpdateRoleAsync(int id, UpdateBoardMemberRoleRequest request)
    {
        var response = await _boardMembersApi.UpdateRoleAsync(id, request);

        return response.HandleResult();
    }
    
    public async Task<Result<DeleteBoardMemberDto>> DeleteAsync(int id)
    {
        var response = await _boardMembersApi.DeleteAsync(id);

        return response.HandleResult();
    }
}