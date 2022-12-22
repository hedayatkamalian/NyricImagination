using Imagination.Server.App.Types;

namespace Imagination.Server.App.Services.Interfaces;

public interface IImageConvertorService
{
    ServiceResult ConvertToJpeg(MemoryStream imageStream);
}
