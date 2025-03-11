using GameServer.Models;

namespace GameServer.Interfaces
{
    public interface IServiceHandler
    {
        string ServiceName { get; }
        object Handle(User user, Dictionary<string, object> details);
    }
}
