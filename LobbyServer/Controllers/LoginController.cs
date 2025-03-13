using LobbyServer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IPlayersRedisService _playersRedisService;
        private readonly string connectionString = string.Empty;


        public LoginController(IPlayersRedisService playersRedisService , IConfiguration configuration)
        {
            _playersRedisService = playersRedisService;
            connectionString = configuration["GameServer:ConnectionString"].ToString();
        }

        [HttpGet("Login/{email}&{password}")]
        public  Dictionary<string, object> Login(string email, string password)
        {

            Dictionary<string, object> result = new Dictionary<string, object>();
            Dictionary<string, string> playerData = _playersRedisService.GetPlayer(email);
            if (playerData.Count > 0 && playerData.ContainsKey("Password"))
            {
                string playerDataPassword = playerData["Password"];
                if (playerDataPassword == password)
                {
                    string userId = playerData["UserId"];

                    result.Add("IsLoggedIn", true);
                    result.Add("UserId", playerData["UserId"]);
                    result.Add("GameServerUrl", connectionString);
                }
                else
                {
                    result.Add("IsLoggedIn", false);
                    result.Add("ErrorMessage", "Wrong Password");
                }
            }
            else
            {
                result.Add("IsLoggedIn", false);
                result.Add("ErrorMessage", "Player Doesnt Exist");
            }

            result.Add("Response", "Login");
            return result;
        }

    }
}
