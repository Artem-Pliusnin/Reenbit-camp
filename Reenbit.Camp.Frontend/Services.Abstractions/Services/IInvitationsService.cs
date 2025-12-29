using Domain.Models.Invitations;
using Domain.Requests.Invitations;
using Domain.Shared;

namespace Services.Abstractions.Services;

public interface IInvitationsService
{
    Task<Result<InvitationModel>> CreateAsync(CreateInvitationRequest request);
    
    Task<Result<List<InvitationModel>>> GetByUserAsync();
    
    Task<Result<object>> AcceptInvitationAsync(int id);
    
    Task<Result<object>> DeclineInvitationAsync(int id);
}