using _7E_Server.Core;
using ImageEditAI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ImageEditAI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImagineArtController : _7E_Controller
    {
        [HttpPost]
        public async Task<Result> Genarate( string prompt)
        {
           
            return await ImagineArtHelper.GenerateImageAsync(prompt);
        }
    }
}