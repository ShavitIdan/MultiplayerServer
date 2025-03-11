using GameServer.Models;

namespace GameServer.Interfaces
{
    public interface IMessageService
    {
        public Task<object> HandleMessageAsync(User curUser,string data);
    }
}
