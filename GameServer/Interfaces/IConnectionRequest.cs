using WebSocketSharp.Server;

namespace GameServer.Interfaces
{
    public interface IConnectionRequest
    {
        Task<bool> OpenAsync(IWebSocketSession session, string id, string details);
    }
}
