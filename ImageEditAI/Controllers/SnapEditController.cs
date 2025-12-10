using _7E_Server.Core;
using ImageEditAI.Helpers;
using ImageEditAI.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<Result> RemoveBackground(IFormFile inputImage)
        {
            return await SnapHelper.RemoveBackground(inputImage);
        }
        [HttpPost]
        public async Task<Result> ImageToBase64(IFormFile inputImage)
        {
            var s = await FileHelpers.ConvertIFormFileToBase64(inputImage);
            return Result.Success(s);
        }
        [HttpPost]
        public async Task<Result> TestMask(IFormFile file)
        {
            var s = await FileHelpers.ConvertIFormFileToBase64(file);
            var path = AppDomain.CurrentDomain.BaseDirectory + "data/monk.json";
            var json = System.IO.File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<snap>(json);
            var original = FileHelpers.Base64ToImage(s);
            var mask = FileHelpers.Base64ToImage(data.output);
           var maskPath = AppDomain.CurrentDomain.BaseDirectory + "data/monk.jpg";
            FileHelpers.SaveImageToFile(mask, maskPath);

            var overlaid = FileHelpers.RemoveBackground(original, mask);
            var resultPath = AppDomain.CurrentDomain.BaseDirectory + "data/overlaid.png";
            FileHelpers.SaveImageToFile(overlaid, resultPath);
            return Result.Success(resultPath);
        }
    [HttpPost]
        public async Task<Result> RemoveBG(IFormFile file)
        {
            var s = await FileHelpers.ConvertIFormFileToBase64(file);
            //var path = AppDomain.CurrentDomain.BaseDirectory + "data/monk.json";
            //var json = System.IO.File.ReadAllText(path);
            var result =  await SnapHelper.RemoveBackground(file);
        
            var data = JsonConvert.DeserializeObject<snap>(result.data as string );
            var original = FileHelpers.Base64ToImage(s);
            var mask = FileHelpers.Base64ToImage(data.output);
           var maskPath = AppDomain.CurrentDomain.BaseDirectory + "data/monk.jpg";
            FileHelpers.SaveImageToFile(mask, maskPath);

            var overlaid = FileHelpers.RemoveBackground(original, mask);
            var resultPath = AppDomain.CurrentDomain.BaseDirectory + "data/snap_overlaid.png";
            FileHelpers.SaveImageToFile(overlaid, resultPath);
            return Result.Success(resultPath);
        }
    }
}