using LobbyServer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisterController : Controller
    {

        private readonly IPlayersRedisService _playersRedisService;


        public RegisterController (IPlayersRedisService playersRedisService)
        {
            _playersRedisService = playersRedisService;
        }

        [HttpPost("register")]
        public ActionResult<Dictionary<string,object>> Register([FromBody] Dictionary<string,object> data)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (data.ContainsKey("Email") && data.ContainsKey("Password"))
            {
                string? email = data["Email"].ToString();
                string? password = data["Password"].ToString();
                Dictionary<string, string> playerData = _playersRedisService.GetPlayer(email);
                if (playerData.Count == 0)
                {
                    string userId = Guid.NewGuid().ToString();
                    Dictionary<string, string> registerData = new Dictionary<string, string>()
                    {
                        {"Email",email},
                        {"Password",password},
                        {"UserId",userId},
                        {"CreatedTime",DateTime.UtcNow.ToString()}
                    };
                    _playersRedisService.SetPlayer(email, registerData);

                    result.Add("IsCreated", true);
                }
                else
                {
                    result.Add("IsCreated", false);
                    result.Add("Message", "User already exists");
                }
            }
            else
            {
                result.Add("IsCreated", false);
                result.Add("Message", "Email and Password are required");
            }
            result.Add("Response", "Register");
         
            return Ok(result);
        }
    }
}
