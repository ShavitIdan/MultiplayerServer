using WebSocketSharp.Server;

namespace GameServer.Interfaces
{
    public interface IProcessRequest
    {
        Task ProcessMessageAsync(IWebSocketSession session, string data);
    }
}
