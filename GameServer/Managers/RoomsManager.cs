using GameServer.Models;

namespace GameServer.Managers
{
    public class RoomsManager
    {
        private Dictionary<string, GameRoom> _activeRooms;
        public Dictionary<string, GameRoom> ActiveRooms { get => _activeRooms; }

        public RoomsManager() 
        { 
            _activeRooms = new Dictionary<string, GameRoom>();
        }

        public void AddRoom(string matchId,GameRoom gameRoom)
        {
            if (_activeRooms == null)
                _activeRooms = new Dictionary<string, GameRoom>();

            if(_activeRooms.ContainsKey(matchId))
                _activeRooms[matchId] = gameRoom;
            else _activeRooms.Add(matchId,gameRoom);
        }

        public void RemoveRoom(string matchId) 
        {
             if(_activeRooms != null && _activeRooms.ContainsKey(matchId))
                _activeRooms.Remove(matchId);
        }

        public GameRoom GetRoom(string matchId)
        {
            if (_activeRooms != null && _activeRooms.ContainsKey(matchId))
                return _activeRooms[matchId];
            return null;
        }

        public bool IsRoomExist(string matchId)
        {
            if(GetRoom(matchId) != null)
                return true;    
            return false;
        }
    }
}
