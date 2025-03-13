using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class GetRoomsInRangeRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;

        public GetRoomsInRangeRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }

        public string ServiceName => "GetRoomsInRange";

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Rooms", new List<Dictionary<string,object>>() },
                { "Response", "GetRoomsInRangeRequest" }
            };
            if (!details.ContainsKey("MinUserCount") || !details.ContainsKey("MaxUserCount"))
            {
                response["Error"] = "Invalid request";
                return response;
            }
            int maxUserCount = int.Parse(details["MaxUserCount"].ToString());
            int minUserCount = int.Parse(details["MinUserCount"].ToString());
            foreach (var gameRoom in _roomManager.ActiveRooms.Values)
            {
                Dictionary<string,object> roomDetails = gameRoom.GetRoomDetails();
                if (roomDetails != null)
                {
                    int userCount = int.Parse(roomDetails["JoinedUserCount"].ToString());
                    if (userCount < maxUserCount && userCount >= minUserCount)
                    {
                        ((List<Dictionary<string, object>>)response["Rooms"]).Add(roomDetails);
                    }
                }
            }

            return response;

        }
    }
}
