
using Domain.Requests.Invitations;
using Domain.Responses.Invitations;
using Refit;

namespace Services.API;

public interface IInvitationApi
{
    [Get("/Invitations")]
    Task<ApiResponse<List<InvitationDto>>> GetByUserAsync();

    [Post("/Invitations")]
    Task<ApiResponse<InvitationDto>> CreateAsync(
        [Body] CreateInvitationRequest request);
    
    [Put("/Invitations/{id}/accept")]
    Task<ApiResponse<object>> AcceptInvitationAsync(int id);
    
    [Put("/Invitations/{id}/decline")]
    Task<ApiResponse<object>> DeclineInvitationAsync(int id);
}