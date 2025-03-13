using System.Collections.Generic;
using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class SendMoveRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;
        public SendMoveRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }

        public string ServiceName => "SendMove";

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            if (details.ContainsKey("MoveData"))
            {
                GameRoom room = _roomManager.GetRoom(user.RoomId);
                if (room != null)
                    response = room.ReceivedMove(user, details["MoveData"].ToString());
            }
            return response;
        }
    }
}
