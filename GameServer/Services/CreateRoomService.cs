using System;
using System.Collections.Generic;
using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services
{
    public class CreateRoomService : ICreateRoomService
    {
        private readonly IMatchIdRedisService _matchIdRedisService;
        private readonly RoomsManager _roomsManager;
        private readonly SessionManager _sessionManager;
        private readonly IRandomizerService _randomizerService;
        private readonly IDateTimeService _dateTimeService;

        public CreateRoomService(IMatchIdRedisService matchIdRedisService, 
            RoomsManager roomsManager, SessionManager sessionManager,
            IRandomizerService randomizerService, IDateTimeService dateTimeService) 
        {
            _matchIdRedisService = matchIdRedisService;
            _roomsManager = roomsManager;
            _sessionManager = sessionManager;
            _randomizerService = randomizerService;
            _dateTimeService = dateTimeService; 
        }

        public Dictionary<string, object> Create(MatchData curMatchData)
        {
            return new Dictionary<string, object>()
            {
                { "Service","StartMatch"},
                { "MatchId",CreateRoom(curMatchData)}
            };
        }

        private object CreateRoom(MatchData curMatchData)
        {
            try
            {
                int dbMatchId = 1;
                string redisMatchId = _matchIdRedisService.GetMatchId();
                if(redisMatchId != null && redisMatchId != string.Empty)
                    dbMatchId = int.Parse(redisMatchId) + 1;

                _matchIdRedisService.SetMatchId(dbMatchId.ToString());

                if(_roomsManager.IsRoomExist(dbMatchId.ToString()) == false)
                {
                    GameRoom gameRoom = new GameRoom(dbMatchId.ToString(), _sessionManager, _roomsManager,
                        _randomizerService, _dateTimeService, curMatchData);

                    _roomsManager.AddRoom(dbMatchId.ToString(), gameRoom);
                    gameRoom.StartGame();
                }

                return dbMatchId.ToString();
            }
            catch (Exception e) { }

            return string.Empty;
        }
    }
}
