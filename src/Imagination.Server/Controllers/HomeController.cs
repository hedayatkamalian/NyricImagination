using Imagination.Server.App.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Imagination.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IImageConvertorService _imageConvertorService;

        public HomeController(IImageConvertorService imageConvertorService)
        {
            _imageConvertorService = imageConvertorService;
        }

        [Route("convert")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Convert()
        {
            var imageStream = new MemoryStream();
            await Request.Body.CopyToAsync(imageStream);
            var result = _imageConvertorService.ConvertToJpeg(imageStream);


            if (result.WasSuccessful)
            {
                return File(result.Result.ToArray(), "image/jpg");
            }

            return UnprocessableEntity(result.Error.ToString());

        }
    }
}
