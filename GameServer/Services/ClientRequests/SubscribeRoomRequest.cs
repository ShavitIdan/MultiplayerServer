using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class SubscribeRoomRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;

        public SubscribeRoomRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }
        public string ServiceName => "SubscribeRoom";

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Response", "SubscribeRoom" },
                { "IsSuccess", false }

            };
            if (!details.ContainsKey("RoomId") || user == null)
            {
                response["Error"] = "Invalid request";
                return response;
            }
            string roomId = details["RoomId"].ToString();

            GameRoom room = _roomManager.GetRoom(roomId);
            if (room == null)
            {
                response["Error"] = "Room not found";
                return response;
            }
            if (room.AddSubscriber(user))
            {
                response["IsSuccess"] = true;
                response["RoomData"] = room.GetRoomDetails();
            }
            else
            {
                response["Error"] = "Failed to subscribe room";
            }

            return response;
        }
    }
}
