using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class CancelMatchingRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;

        public CancelMatchingRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }

        public string ServiceName => "CancelMatching";

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Response", "CancelMatching" },
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
            room.RemoveUser(user);
            response["IsSuccess"] = true;
            return response;

        }
    }
}
