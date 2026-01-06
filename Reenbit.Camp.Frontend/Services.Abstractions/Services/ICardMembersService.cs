using Domain.Models.CardMembers;
using Domain.Requests.CardMembers;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface ICardMembersService
{
    Task<Result<List<CardMemberModel>>> GetByCardAsync(int cardId);
    
    Task<Result<CardMemberModel>> CreateAsync(CreateCardMemberRequest request);
    
    Task<Result<object>> DeleteAsync(int id);
}
