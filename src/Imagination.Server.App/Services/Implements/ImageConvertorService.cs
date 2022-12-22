using Imagination.Server.App.Options;
using Imagination.Server.App.Services.Interfaces;
using Imagination.Server.App.Types;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Imagination.Server.App.Services.Implements;

public class ImageConvertorService : IImageConvertorService
{
    private readonly ApplicationOptions _applicationOptions;

    public ImageConvertorService(IOptions<ApplicationOptions> applicationOptions)
    {
        _applicationOptions = applicationOptions.Value;
    }

    public ServiceResult ConvertToJpeg(MemoryStream imageStream)
    {
        using (imageStream)
        {
            if (imageStream is null || imageStream.Length == 0)
            {
                return new ServiceResult(ServiceErrorType.EmptyOrNullStream);
            }

            var codec = SKCodec.Create(new MemoryStream(imageStream.ToArray()));


            if (codec is null)
            {
                return new ServiceResult(ServiceErrorType.EmptyImageOrUnknownFormat);
            }

            var bitmap = new SKBitmap(codec.Info);

            if (bitmap.Width > _applicationOptions.MaxDimensionSize || bitmap.Height > _applicationOptions.MaxDimensionSize)
            {
                var newSize = new ImageSize(bitmap.Width, bitmap.Height).FitIntoSquare(_applicationOptions.MaxDimensionSize);
                bitmap = bitmap.Resize(new SKImageInfo(newSize.Width, newSize.Height), SKFilterQuality.None);
            }

            using (var jpegStream = new MemoryStream())
            {
                SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Jpeg, _applicationOptions.JpegQuality).AsStream(false).CopyTo(jpegStream);
                return new ServiceResult(jpegStream);
            }

        }

    }
}
