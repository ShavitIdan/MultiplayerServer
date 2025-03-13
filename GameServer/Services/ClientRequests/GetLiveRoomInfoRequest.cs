using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class GetLiveRoomInfoRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;

        public string ServiceName => "GetLiveRoomInfo";

        public GetLiveRoomInfoRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }
        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Response", "GetLiveRoomInfo" },
                { "IsSuccess", false }
            };

            if (!details.ContainsKey("RoomId"))
            {
                response["Error"] = "Missing RoomId";
                return response;
            }

            string roomId = details["RoomId"].ToString();

            if (!_roomManager.IsRoomExist(roomId))
            {
                response["Error"] = "Room not found";
                return response;
            }
            GameRoom room = _roomManager.GetRoom(roomId);

            response["IsSuccess"] = true;
            response["RoomData"] = room.GetRoomDetails();
            response["RoomProperties"] = room.GetRoomProperties();
            response["Users"] = room.GetJoinedUsersList();

            return response;
        }
    }


}