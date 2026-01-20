using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using AutoMapper;
using Domain.Constants.FIleConstants;
using Domain.DTOs.UserAvatars;
using Domain.Entities;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Users.Commands.UpdateUserAvatar;

internal class UpdateUserAvatarCommandHandler : ICommandHandler<UpdateUserAvatarCommand, UserAvatarDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public UpdateUserAvatarCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }
    
    public async Task<Result<UserAvatarDto>> Handle(
        UpdateUserAvatarCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            var fileDto = await _fileService
                .UploadFileAsync(
                    request.FileContent,
                    request.FileName,
                    request.ContentType,
                    FileDirectoriesConstants.Avatars,
                    cancellationToken);

            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Result.Failure<UserAvatarDto>(UserErrors.UserDoesNotExistError);
            }
            
            var userAvatarRepository = _unitOfWork.GetRepository<IUserAvatarRepository>();

            UserAvatar userAvatar;

            if (user.Avatar == null)
            {
                userAvatar = new UserAvatar()
                {
                    UserId = user.Id,
                    FileName = fileDto.FileName,
                    FileUrl = fileDto.FileUrl,
                };
                
                userAvatarRepository.Add(userAvatar);
            }
            else
            {
                userAvatar = user.Avatar;
                
                await _fileService
                    .DeleteFileAsync(
                        userAvatar.FileName, 
                        FileDirectoriesConstants.Avatars, 
                        cancellationToken);
                
                userAvatar.FileName = fileDto.FileName;
                userAvatar.FileUrl = fileDto.FileUrl;
                
                userAvatarRepository.Update(userAvatar);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<UserAvatarDto>(userAvatar);

            return response;
        }
        catch (FileStorageException ex)
        {
            return Result.Failure<UserAvatarDto>(UserAvatarErrors.SavingFileError);
        }
        catch
        {
            return Result.Failure<UserAvatarDto>(UserAvatarErrors.UpdateUserAvatarError);
        }
    }
}