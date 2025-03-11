namespace GameServer.Interfaces.WebSocketInterfaces
{
    public interface ICloseRequest
    {
        Task CloseAsync (string id);
    }
}
