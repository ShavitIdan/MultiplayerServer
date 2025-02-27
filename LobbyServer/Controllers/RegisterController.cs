using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
