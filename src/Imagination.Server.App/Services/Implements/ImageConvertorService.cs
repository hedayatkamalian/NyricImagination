using Imagination.Server.App.Services.Interfaces;
using Imagination.Server.App.Types;
using SkiaSharp;

namespace Imagination.Server.App.Services.Implements;

public class ImageConvertorService : IImageConvertorService
{
    public ServiceResult ConvertToJpeg(MemoryStream imageStream)
    {
        if (imageStream is null || imageStream.Length == 0)
        {
            return new ServiceResult(ServiceErrorType.EmptyOrNullStream);
        }

        if (SKBitmap.DecodeBounds(imageStream).IsEmpty)
        {
            return new ServiceResult(ServiceErrorType.EmptyImageOrUnknownFormat);
        }


        return new ServiceResult(new MemoryStream());
    }
}
