using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class StartGameRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;

        public StartGameRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }
        public string ServiceName => "StartGame";

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Response", "StartGame" },
                { "IsSuccess", false }
            };
            if (user == null)
            {
                response["Error"] = "Invalid request";
                return response;
            }
            string roomId = user.RoomId;
            if (!_roomManager.IsRoomExist(roomId))
            {
                response["Error"] = "Room not found";
                return response;
            }
            GameRoom room = _roomManager.GetRoom(roomId);
            room.StartGame(user.UserId);
            response["IsSuccess"] = true;
            return response;
        }
    }
}
