using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.DTOs.Labels;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Labels.Commands.UpdateLabel;

internal class UpdateLabelCommandHandler : ICommandHandler<UpdateLabelCommand, UpdatedLabelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLabelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result<UpdatedLabelDto>> Handle(
        UpdateLabelCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var labelRepository = _unitOfWork.GetRepository<ILabelRepository>();

            var label = await labelRepository
                .GetByIdAsync(request.LabelId, cancellationToken);

            if (label == null)
            {
                return Result.Failure<UpdatedLabelDto>(LabelErrors.LabelDoesNotExistError);
            }

            label.Text = request.Text;
            label.Color = request.Color;

            labelRepository.Update(label);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var response = _mapper.Map<UpdatedLabelDto>(label);

            return Result.Success(response);
        }
        catch
        {
            return Result.Failure<UpdatedLabelDto>(LabelErrors.UpdateLabelError);
        }
    }
}