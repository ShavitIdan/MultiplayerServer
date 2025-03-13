namespace GameServer.Interfaces
{
    public interface IRoomIdRedisService
    {
        string GetRoomId();
        void SetRoomId(string roomId);    
    }
}
