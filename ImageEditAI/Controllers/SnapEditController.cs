using _7E_Server.Core;
using ImageEditAI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ImageEditAI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SnapEditController : _7E_Controller
    {
        [HttpGet]
        public Result Test()
        {
            return Result.Success();
        }
        [HttpGet]
        public async Task<Result> AutoSuggest()
        {
            return await SnapHelper.AutoSuggest();
        }
        [HttpPost]
        public async Task<Result> RemoveBackground( IFormFile inputImage)
        {
            return await SnapHelper.RemoveBackground(inputImage);
        }
     [HttpPost]
        public async Task<Result> ImageToBase64( IFormFile inputImage)
        {
            var s = await FileHelpers.ConvertIFormFileToBase64(inputImage);
            return Result.Success(s);
        }}
}