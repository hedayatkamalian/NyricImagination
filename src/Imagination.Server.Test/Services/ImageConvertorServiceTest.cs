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


    [Theory]
    [InlineData("resources/badformat.txt")]
    public async Task ConvertToJpeg_Should_Return_EmptyImageOrUnknownFormat_Error_When_Image_Is_Empty_Or_Has_Invalid_Format(string fileAddress)
    {
        var imageConvertorService = new ImageConvertorService();
        var result = imageConvertorService.ConvertToJpeg(ReadFileAsStream(fileAddress));

        result.Should().NotBeNull();
        result.WasSuccessful.Should().BeFalse();
        result.Error.Should().Be(ServiceErrorType.EmptyImageOrUnknownFormat);
    }


    private MemoryStream ReadFileAsStream(string filePath)
    {
        return new MemoryStream(File.ReadAllBytes(filePath));
    }
}
