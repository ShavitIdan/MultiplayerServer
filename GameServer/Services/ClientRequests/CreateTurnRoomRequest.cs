using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class CreateTurnRoomRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;

        public string ServiceName => "CreateTurnRoom";

        public CreateTurnRoomRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Response", "CreateTurnRoomRequest" }
            };
            
            if (!details.ContainsKey("_MaxUsers") || !details.ContainsKey("_TurnTime"))
            {
                response["Error"] = "Invalid request";
                return response;
            }



            return response;
        }
    }
}
