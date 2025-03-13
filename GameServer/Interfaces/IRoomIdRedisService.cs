namespace GameServer.Interfaces
{
    public interface IRoomIdRedisService
    {
        string GetMatchId();
        void SetMatchId(string matchId);    
    }
}
