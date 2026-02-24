
using Domain.Requests.Invitations;
using Domain.Responses.Invitations;
using Refit;

namespace Services.API;

public interface IInvitationApi
{
    [Get("/Invitations")]
    Task<ApiResponse<List<InvitationDto>>> GetByUserAsync();
    
    [Get("/Invitations/board/{boardId}")]
    Task<ApiResponse<List<InvitationDto>>> GetByBoardAsync(int boardId);

    [Post("/Invitations/board/{boardId}")]
    Task<ApiResponse<InvitationDto>> CreateAsync(
        int boardId,
        [Body] CreateInvitationRequest request);
    
    [Put("/Invitations/{id}/accept")]
    Task<ApiResponse<object>> AcceptInvitationAsync(int id);
    
    [Put("/Invitations/{id}/decline")]
    Task<ApiResponse<object>> DeclineInvitationAsync(int id);
    
    [Delete("/Invitations/board/{boardId}/invitation/{id}")]
    Task<ApiResponse<object>> DeleteAsync(int boardId, int id);
}