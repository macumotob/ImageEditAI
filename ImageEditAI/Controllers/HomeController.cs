using _7E_Server.Core;
using ImageEditAI.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ImageEditAI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HomeController : _7E_Controller
    {
        [HttpGet]
        public Result GetInterfaces()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "data/interface.json";
           var json = System.IO.File.ReadAllText(path);
           var data = JsonConvert.DeserializeObject<List<client_interface>>(json);
            return Result.Success(data);;
        }
    }
}