using GameServer.Interfaces.WebSocketInterfaces;
using GameServer.Managers;
using GameServer.Models;
using GameServer.Services.Redis;

namespace GameServer.Services.Requests
{
    public class CloseRequest : ICloseRequest
    {
        private readonly SessionManager _sessionManager;
        private readonly IdToUserIdManager _idToUserIdManager;
        public CloseRequest(SessionManager sessionManager,
            IdToUserIdManager idToUserIdManager)
        {
            _sessionManager = sessionManager;
            _idToUserIdManager = idToUserIdManager;
        }


        public Task CloseAsync(string id)
        {
            string userId = _idToUserIdManager.GetUserId(id);

            User curUser = _sessionManager.GetUser(userId);
            curUser.CurUserState = User.UserState.Disconnected;
            _sessionManager.UpdateUser(curUser);

            return Task.CompletedTask;
        }
    }
}
