using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class LeaveRoomRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;
        public LeaveRoomRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }

        public string ServiceName => "LeaveRoom";

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Response", "LeaveRoom" },
                { "IsSuccess", false }
            };

            if (!details.ContainsKey("RoomId") || user == null)
            {
                response["Error"] = "Invalid request: Missing RoomId or User";
                return response;
            }
            string roomId = details["RoomId"].ToString();
            GameRoom room = _roomManager.GetRoom(roomId);
            if (room == null)
            {
                response["Error"] = "Room not found";
                return response;
            }
            if (room.RemoveUser(user))
            {
                Dictionary<string, object> roomDetails = room.GetRoomDetails();

                response["IsSuccess"] = true;
                response["RoomId"] = roomDetails == null ? "0" : roomId;
                response["Owner"] = roomDetails == null ? "" : roomDetails["Owner"];
                response["MaxUsers"] = roomDetails == null ? 0 : roomDetails["MaxUsersCount"];
                response["Name"] = roomDetails == null ? "" : roomDetails["Name"];
            }
            else
            {
                response["Error"] = "Failed to leave room";


            }
            return response;
        }
    }
}
