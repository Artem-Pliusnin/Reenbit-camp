using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Labels;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Labels.Commands.CreateLabel;

internal class CreateLabelCommandHandler : ICommandHandler<CreateLabelCommand, LabelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLabelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<LabelDto>> Handle(
        CreateLabelCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var labelRepository = _unitOfWork.GetRepository<ILabelRepository>();

            var label = new Label()
            {
                BoardId = request.BoardId,
                Text = request.Text,
                Color = request.Color
            };
            
            labelRepository.Add(label);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<LabelDto>(label);

            return response;
        }
        catch
        {
            return Result.Failure<LabelDto>(LabelErrors.CreateLabelError);
        }
    }
}