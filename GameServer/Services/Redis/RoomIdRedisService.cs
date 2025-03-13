using GameServer.Interfaces;
using GameServer.Models;

namespace GameServer.Services.Redis
{
    public class RoomIdRedisService : IRoomIdRedisService
    {

        private readonly IRedisBaseService _redisBaseService;

        public RoomIdRedisService(IRedisBaseService redisBaseService)
        {
            _redisBaseService = redisBaseService;
        }

        public string GetMatchId()
        {
            return _redisBaseService.GetString("DBMatchId2");
        }

        public void SetMatchId(string matchId)
        {
            _redisBaseService.SetString("DBMatchId2", matchId);
        }
    }
}
