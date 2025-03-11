using GameServer.Interfaces.WebSocketInterfaces;
using GameServer.Managers;
using GameServer.Models;
using GameServer.Services.Redis;

namespace GameServer.Services.Requests
{
    public class CloseRequest : ICloseRequest
    {
        private readonly SessionManager _sessionManager;
        private readonly SearchingManager _searchingManager;
        private readonly IdToUserIdManager _idToUserIdManager;
        public CloseRequest(SessionManager sessionManager,
            SearchingManager searchingManager,
            IdToUserIdManager idToUserIdManager)
        {
            _sessionManager = sessionManager;
            _searchingManager = searchingManager;
            _idToUserIdManager = idToUserIdManager;
        }


        public Task CloseAsync(string id)
        {
            string userId = _idToUserIdManager.GetUserId(id);
            _searchingManager.RemoveToSearch(userId);

            User curUser = _sessionManager.GetUser(userId);
            curUser.CurUserState = User.UserState.Disconnected;
            _sessionManager.UpdateUser(curUser);

            return Task.CompletedTask;
        }
    }
}
