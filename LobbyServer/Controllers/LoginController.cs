using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : Controller
    {
        [HttpPost("login")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
