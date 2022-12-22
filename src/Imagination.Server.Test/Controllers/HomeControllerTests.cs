using AutoFixture;
using FluentAssertions;
using Imagination.Controllers;
using Imagination.Server.App.Services.Interfaces;
using Imagination.Server.App.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Imagination.Server.Test.Controllers
{
    public class HomeControllerTests
    {
        private Fixture _fixture { get { return new Fixture(); } }

        [Theory]
        [InlineData(ServiceErrorType.EmptyOrNullStream)]
        [InlineData(ServiceErrorType.EmptyImageOrUnknownFormat)]
        [InlineData(ServiceErrorType.Unknown)]
        public async Task Convert_Should_Return_UnSupportable_Enitity_When_ServiceResult_Has_Error(ServiceErrorType error)
        {

            var serviceResult = new ServiceResult(error);
            var imaginationServiceMock = new Mock<IImageConvertorService>();
            imaginationServiceMock.Setup(x => x.ConvertToJpeg(It.IsAny<MemoryStream>())).Returns(serviceResult);

            var homeController = CreateHomeControllerWithHttpContext(new MemoryStream(), imaginationServiceMock);

            var result = await homeController.Convert(_fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<UnprocessableEntityObjectResult>();

        }

        [Fact]
        public async Task Convert_Should_Return_File_When_ServiceResult_Is_Successfull()
        {

            var serviceResult = new ServiceResult(new MemoryStream());
            var imaginationServiceMock = new Mock<IImageConvertorService>();
            imaginationServiceMock.Setup(x => x.ConvertToJpeg(It.IsAny<MemoryStream>())).Returns(serviceResult);

            var homeController = CreateHomeControllerWithHttpContext(new MemoryStream(), imaginationServiceMock);

            var result = await homeController.Convert(_fixture.Create<CancellationToken>());

            result.Should().NotBeNull();
            result.Should().BeOfType<FileContentResult>();

        }


        public HomeController CreateHomeControllerWithHttpContext(Stream stream, Mock<IImageConvertorService> mockedService)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Body = stream;
            httpContext.Request.ContentLength = 0;

            return new HomeController(mockedService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }
    }
}
