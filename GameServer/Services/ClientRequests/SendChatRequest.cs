using System.Collections.Generic;
using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class SendChatRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;
        public string ServiceName => "SendChat";

        public SendChatRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }

        public object Handle(User user, Dictionary<string, object> details)
        {
            if (!details.ContainsKey("Message"))
                return new Dictionary<string, object>() { { "Error","MessageRequired" } };

            string message = details["Message"].ToString();
            GameRoom room = _roomManager.GetRoom(user.RoomId);
            room.SendChat(user, message);
            return new Dictionary<string, object>(); ;
        }
    }
}
