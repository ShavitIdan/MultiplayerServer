using GameServer.Managers;

namespace GameServer.Interfaces
{
    public interface ICreateRoomService
    {
        Dictionary<string, object> Create(MatchData curMatchData);
    }
}
