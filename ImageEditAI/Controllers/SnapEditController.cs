using _7E_Server.Core;
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
    }
}
