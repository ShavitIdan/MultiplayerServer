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
                { "Response", "GetRoomsInRange" }
            };
            if (!details.ContainsKey("MinUserCount") || !details.ContainsKey("MaxUserCount"))
            {
                response["Error"] = "Invalid request";
                return response;
            }
            int maxUserCount = int.Parse(details["MaxUserCount"].ToString());
            int minUserCount = int.Parse(details["MinUserCount"].ToString());

            Console.WriteLine("[Server] Active Rooms:");
            foreach (var room in _roomManager.ActiveRooms.Values)
            {
                Console.WriteLine($"  - Room ID: {room.GetRoomDetails()["RoomId"]}, Users: {room.GetRoomDetails()["JoinedUsersCount"]}");
            }

            foreach (var gameRoom in _roomManager.ActiveRooms.Values)
            {
                Dictionary<string,object> roomDetails = gameRoom.GetRoomDetails();
                if (roomDetails != null)
                {
                    if (!int.TryParse(roomDetails["JoinedUsersCount"].ToString(), out int userCount))
                    {
                        Console.WriteLine("[ERROR] Invalid JoinedUsersCount format.");
                        continue;
                    }


                    if (userCount < maxUserCount && userCount >= minUserCount)
                    {
                        ((List<Dictionary<string, object>>)response["Rooms"]).Add(roomDetails);
                    }
                }
            }
            Console.WriteLine($"[Server] Found {((List<Dictionary<string, object>>)response["Rooms"]).Count} rooms matching the criteria.");


            return response;

        }
    }
}
