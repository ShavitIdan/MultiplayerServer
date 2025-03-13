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

        public string GetRoomId()
        {
            return _redisBaseService.GetString("DBRoomId");
        }

        public void SetRoomId(string roomId)
        {
            _redisBaseService.SetString("DBRoomId", roomId);
        }
    }
}
