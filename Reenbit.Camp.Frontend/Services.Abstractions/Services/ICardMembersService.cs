using Domain.Models.CardMembers;
using Domain.Requests.CardMembers;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ICardMembersService
{
    Task<Result<List<CardMemberModel>>> GetByCardAsync(int boardId, int cardId);
    
    Task<Result<CardMemberModel>> CreateAsync(int boardId, CreateCardMemberRequest request);
    
    Task<Result<object>> DeleteAsync(int boardId, int id);
}
