using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
