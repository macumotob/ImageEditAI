using _7E_Server.Core;
using ImageEditAI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ImageEditAI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClipdropController : _7E_Controller
    {
   
        [HttpPost]
        public async Task<Result> UncropImage(IFormFile imageFile, int extendLeft, int extendDown)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return Result.Error("No image file uploaded.");
            }
            return await ClipdropHelper.UncropImage(imageFile, extendLeft, extendDown);

        }

    }
}
