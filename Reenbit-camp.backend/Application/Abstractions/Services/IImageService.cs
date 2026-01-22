namespace Application.Abstractions.Services;

public interface IImageService
{
    Task<Stream> ResizeAvatarAsync(
        Stream input,
        int size,
        CancellationToken cancellationToken);
}