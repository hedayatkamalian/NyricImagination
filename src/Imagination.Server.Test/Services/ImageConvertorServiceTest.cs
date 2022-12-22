using FluentAssertions;
using Imagination.Server.App.Options;
using Imagination.Server.App.Services.Implements;
using Imagination.Server.App.Types;
using Microsoft.Extensions.Options;
using Moq;
using SkiaSharp;

namespace Imagination.Server.Test.Services;

public class ImageConvertorServiceTest
{
    [Fact]
    public void ConvertToJpeg_Should_Return_EmptyOrNullStream_Error_When_Image_Has_No_Data()
    {
        var stream = new MemoryStream();
        var applicationOptions = GetMockOptions();
        var imageConvertorService = new ImageConvertorService(applicationOptions.Object);
        var result = imageConvertorService.ConvertToJpeg(stream);

        result.Should().NotBeNull();
        result.WasSuccessful.Should().BeFalse();
        result.Error.Should().Be(ServiceErrorType.EmptyOrNullStream);
    }


    [Theory]
    [InlineData("resources/badformat.txt")]
    public void ConvertToJpeg_Should_Return_EmptyImageOrUnknownFormat_Error_When_Image_Is_Empty_Or_Has_Invalid_Format(string fileAddress)
    {
        var applicationOptions = GetMockOptions();
        var imageConvertorService = new ImageConvertorService(applicationOptions.Object);
        var result = imageConvertorService.ConvertToJpeg(ReadFileAsStream(fileAddress));

        result.Should().NotBeNull();
        result.WasSuccessful.Should().BeFalse();
        result.Error.Should().Be(ServiceErrorType.EmptyImageOrUnknownFormat);
    }

    [Theory]
    [InlineData("resources/big.jpg")]
    [InlineData("resources/small.png")]
    [InlineData("resources/tall.jpg")]
    public void ConvertToJpeg_Should_Produce_Images_Which_Fit_In_Square_With_MaxDimentionSize(string fileAddress)
    {
        var applicationOptions = GetMockOptions();
        var imageConvertorService = new ImageConvertorService(applicationOptions.Object);
        var result = imageConvertorService.ConvertToJpeg(ReadFileAsStream(fileAddress));

        result.Should().NotBeNull();
        result.WasSuccessful.Should().BeTrue();

        var bitamp = SKBitmap.Decode(result.Result.ToArray());
        bitamp.Width.Should().BeLessThanOrEqualTo(applicationOptions.Object.Value.MaxDimensionSize);
        bitamp.Height.Should().BeLessThanOrEqualTo(applicationOptions.Object.Value.MaxDimensionSize);

    }

    [Theory]
    [InlineData("resources/invalid.png")]
    public void ConvertToJpeg_Should_Convert_Images_With_Unexpected_End_Of_File(string fileAddress)
    {
        var applicationOptions = GetMockOptions();
        var imageConvertorService = new ImageConvertorService(applicationOptions.Object);
        var result = imageConvertorService.ConvertToJpeg(ReadFileAsStream(fileAddress));

        result.Should().NotBeNull();
        result.WasSuccessful.Should().BeTrue();
    }

    [Theory]
    [InlineData("resources/jfif.jfif")]
    public void ConvertToJpeg_Should_Convert_Images_With_Invalid_Extentions(string fileAddress)
    {
        var applicationOptions = GetMockOptions();
        var imageConvertorService = new ImageConvertorService(applicationOptions.Object);
        var result = imageConvertorService.ConvertToJpeg(ReadFileAsStream(fileAddress));

        result.Should().NotBeNull();
        result.WasSuccessful.Should().BeTrue();
    }

    [Theory]
    [InlineData("resources/transparent.png")]
    public void ConvertToJpeg_Should_Convert_Transparent_Images(string fileAddress)
    {
        var applicationOptions = GetMockOptions();
        var imageConvertorService = new ImageConvertorService(applicationOptions.Object);
        var result = imageConvertorService.ConvertToJpeg(ReadFileAsStream(fileAddress));

        result.Should().NotBeNull();
        result.WasSuccessful.Should().BeTrue();
    }


    private Mock<IOptions<ApplicationOptions>> GetMockOptions(int maxDimention = 1024)
    {
        var option = new Mock<IOptions<ApplicationOptions>>();
        option.SetupGet(o => o.Value).Returns(new ApplicationOptions
        {
            JpegQuality = 75,
            MaxDimensionSize = maxDimention
        });

        return option;
    }

    private MemoryStream ReadFileAsStream(string filePath)
    {
        return new MemoryStream(File.ReadAllBytes(filePath));
    }
}
