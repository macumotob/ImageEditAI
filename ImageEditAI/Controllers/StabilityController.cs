using _7E_Server.Core;
using ImageEditAI.Helpers;
using ImageEditAI.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ImageEditAI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StabilityController : _7E_Controller
    {
        [HttpPost]
        public async Task<Result> EditImage(string prompt, string search_prompt, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return Result.Error("No image file provided.");
            }
            return await StabilityHelper.EditImage(prompt, search_prompt, image);
        }
    }
}