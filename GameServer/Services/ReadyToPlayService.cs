using System;
using System.Collections.Generic;
using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services
{
    public class ReadyToPlayService : IServiceHandler
    {
        private readonly MatchingManager _matchingManager;
        private readonly ICreateRoomService _createRoomService;
        public string ServiceName => "ReadyToPlay";

        public ReadyToPlayService(MatchingManager matchingManager, ICreateRoomService createRoomService) 
        {
            _matchingManager = matchingManager;
            _createRoomService = createRoomService;
        }

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            if (details.ContainsKey("MatchId"))
            {
                bool isReady = ReadyToPlayLogic(user, details["MatchId"].ToString());
                response.Add("IsSuccess", isReady);
            }
            else response.Add("IsSuccess", false);
            return response;
        }

        private bool ReadyToPlayLogic(User user,string matchId)
        {
            MatchData matchData = _matchingManager.GetMatchData(matchId);
            if(matchData != null)
            {
                matchData.ChangePlayerReady(user.UserId,true);
                if(matchData.IsAllReady())
                {
                    _matchingManager.RemoveFromMatchingData(matchId);
                    _createRoomService.Create(matchData);
                    Console.WriteLine("Create Room");
                }
                return true;
            }
            return false;
        }
    }
}
