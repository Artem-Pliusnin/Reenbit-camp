using Domain.Models.Invitations;
using Domain.Requests.Invitations;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IInvitationsService
{
    Task<Result<InvitationModel>> CreateAsync(int boardId, CreateInvitationRequest request);
    
    Task<Result<List<InvitationModel>>> GetByUserAsync();
    
    Task<Result<List<InvitationModel>>> GetByBoardAsync(int boardId);
    
    Task<Result<object>> AcceptInvitationAsync(int id);
    
    Task<Result<object>> DeclineInvitationAsync(int id);
    
    Task<Result<object>> DeleteAsync(int boardId, int id);
}