using FluentAssertions;
using Imagination.Server.App.Services.Implements;
using Imagination.Server.App.Types;

namespace Imagination.Server.Test.Services;

public class ImageConvertorServiceTest
{
    [Fact]
    public async Task ConvertToJpeg_Should_Return_EmptyOrNullStream_Error_When_Image_Has_No_Data()
    {
        var stream = new MemoryStream();
        var imageConvertorService = new ImageConvertorService();
        var result = imageConvertorService.ConvertToJpeg(stream);

        result.Should().NotBeNull();
        result.WasSuccessful.Should().BeFalse();
        result.Error.Should().Be(ServiceErrorType.EmptyOrNullStream);
    }
}
