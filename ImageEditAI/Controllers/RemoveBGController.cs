using _7E_Server.Core;
using ImageEditAI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ImageEditAI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RemoveBGController : _7E_Controller
    {
        [HttpPost]
        public async Task<Result> RemoveBackground( IFormFile inputImage)
        {
            if (inputImage == null || inputImage.Length == 0)
            {
                return Result.Error("No file uploaded.");
            }

            return await RemoveBGHelper.RemoveBackground(inputImage);
        }
    }
}