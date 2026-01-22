using Application.Abstractions.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Services;

public class ImageService : IImageService
{
    public async Task<Stream> ResizeAvatarAsync(Stream input, int size, CancellationToken cancellationToken)
    {
        input.Position = 0;
        using var image = await Image.LoadAsync(input, cancellationToken);
        
        var minSide = Math.Min(image.Width, image.Height);
        var centeredX = (image.Width - minSide) / 2;
        var centeredY = (image.Height - minSide) / 2;
            
        image.Mutate(img => 
            img.Crop(new Rectangle(
                    centeredX, 
                    centeredY, 
                    minSide, 
                    minSide))
                .Resize(size, size));
        
        var output = new MemoryStream();
        await image.SaveAsPngAsync(output, cancellationToken);
        
        output.Position = 0;
        return output;
    }
}