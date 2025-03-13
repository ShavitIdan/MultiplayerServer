using Newtonsoft.Json;
using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Models;
using WebSocketSharp.Server;

namespace GameServer.Services
{
    public class ConnectionRequest : IConnectionRequest
    {
        private readonly SessionManager _sessionManager;
        private readonly IdToUserIdManager _idToUserIdManager;

        public ConnectionRequest(SessionManager sessionManager,
            IdToUserIdManager idToUserIdManager)
        {
            _sessionManager = sessionManager;
            _idToUserIdManager = idToUserIdManager;
        }

        public Task<bool> OpenAsync(IWebSocketSession session, string id, string details)
        {
            Console.WriteLine(id + " " + details);
            try
            {
                Dictionary<string,string> userData = JsonConvert.DeserializeObject<Dictionary<string,string>>(details);
                if (userData != null && userData.ContainsKey("UserId"))
                {
                    string userId = userData["UserId"];
                    User newUser = new User(userId,session);
                    newUser.SetMatchingState();

                    _sessionManager.AddUser(newUser);
                    _idToUserIdManager.AddMapping(session.ID,userId);
                    return Task.FromResult(true);
                    
                }
            }
            catch (Exception ex) { }
            return Task.FromResult(false);
        }
    }
}
