using Application.Abstractions.Messaging;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Labels.Commands.UpdateLabel;

internal class UpdateLabelCommandHandler : ICommandHandler<UpdateLabelCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLabelCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(
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
                return Result.Failure(LabelErrors.LabelDoesNotExistError);
            }

            label.Text = request.Text;
            label.Color = request.Color;

            labelRepository.Update(label);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(LabelErrors.UpdateLabelError);
        }
    }
}