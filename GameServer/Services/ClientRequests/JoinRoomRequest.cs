﻿using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services.ClientRequests
{
    public class JoinRoomRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;
        private readonly SessionManager _sessionManager;

        public JoinRoomRequest(RoomsManager roomManager, SessionManager sessionManager)
        {
            _roomManager = roomManager;
            _sessionManager = sessionManager;
        }
        public string ServiceName => "JoinRoom";

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                { "Response", "JoinRoom" },
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
           
            if (room.TryAddUser(user))
            {
                response["IsSuccess"] = true;
                response["RoomId"] = roomId;
            }
            else
            {
                response["Error"] = "Failed to join room";
            }
            return response;
        }
    }
}
