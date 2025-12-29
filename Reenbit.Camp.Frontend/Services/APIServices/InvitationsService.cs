using AutoMapper;
using Domain.Models.Invitations;
using Domain.Requests.Invitations;
using Domain.Shared;
using Services.Abstractions.Services;
using Services.API;
using Services.Extensions;

namespace Services.APIServices;

public class InvitationsService : IInvitationsService
{
    private readonly IInvitationApi _invitationApi;
    private readonly IMapper _mapper;

    public InvitationsService(IInvitationApi invitationApi, IMapper mapper)
    {
        _invitationApi = invitationApi;
        _mapper = mapper; 
    }
    
    public async Task<Result<InvitationModel>> CreateAsync(CreateInvitationRequest request)
    {
        var response = await _invitationApi.CreateAsync(request);
        
        return response.HandleResultWithMapping(content 
            => _mapper.Map<InvitationModel>(content));
    }

    public async Task<Result<List<InvitationModel>>> GetByUserAsync()
    {
        var response = await _invitationApi.GetByUserAsync();

        return response.HandleResultWithMapping(content 
            => _mapper.Map<List<InvitationModel>>(content));
    }

    public async Task<Result<object>> AcceptInvitationAsync(int id)
    {
        var response = await _invitationApi.AcceptInvitationAsync(id);

        return response.HandleResult();
    }

    public async Task<Result<object>> DeclineInvitationAsync(int id)
    {
        var response = await _invitationApi.DeclineInvitationAsync(id);

        return response.HandleResult();
    }
}