using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace GameServer.Services.ClientRequests
{
    public class CreateTurnRoomRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;
        private readonly SessionManager _sessionManager;
        private readonly IRandomizerService _randomizerService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRoomIdRedisService _roomIdRedisService;
        public string ServiceName => "CreateTurnRoom";

        public CreateTurnRoomRequest(RoomsManager roomManager, SessionManager sessionManager,
                                     IRandomizerService randomizerService, IDateTimeService dateTimeService, IRoomIdRedisService roomIdRedisService)
        {
            _roomManager = roomManager;
            _sessionManager = sessionManager;
            _randomizerService = randomizerService;
            _dateTimeService = dateTimeService;
            _roomIdRedisService = roomIdRedisService;
        }

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Response", "CreateTurnRoom" }
            };
            
            if (!details.ContainsKey("Name") || !details.ContainsKey("Owner") || !details.ContainsKey("MaxUsers")
                || !details.ContainsKey("TableProperties") || !details.ContainsKey("TurnTime"))
            {
                response["Error"] = "Invalid request";
                return response;
            }
            if (!int.TryParse(details["MaxUsers"].ToString(), out int maxUsers) ||
                    !int.TryParse(details["TurnTime"].ToString(), out int turnTime))
            {
                response["Error"] = "Invalid MaxUsers or TurnTime value.";
                return response;
            }
            string name = details["Name"].ToString();
            string owner = details["Owner"].ToString();
            Dictionary<string, object> tableProperties = JsonConvert.DeserializeObject<Dictionary<string,object>>( details["TableProperties"].ToString());


            
            try
            {
                string roomId = _roomIdRedisService.GetRoomId() == null ? "1" : _roomIdRedisService.GetRoomId();
                GameRoom newRoom = new GameRoom(roomId, _roomManager, _randomizerService, _dateTimeService, _sessionManager, name, owner, maxUsers, tableProperties);
               
                _roomManager.AddRoom(roomId, newRoom);

                response["RoomId"] = roomId;
                response["IsSuccess"] = true;

                _roomIdRedisService.SetRoomId((int.Parse(roomId) + 1).ToString());


            }
            catch (Exception ex)
            {
                response["IsSuccess"] = false;
                response["Error"] = ex.Message;
            }
           

            return response;
        }
    }
}
