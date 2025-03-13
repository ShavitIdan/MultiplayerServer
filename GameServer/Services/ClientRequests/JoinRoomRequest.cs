//using GameServer.Interfaces;
//using GameServer.Managers;
//using GameServer.Models;

//namespace GameServer.Services.ClientRequests
//{
//    public class JoinRoomRequest : IServiceHandler
//    {
//        private readonly RoomsManager _roomManager;
//        private readonly SessionManager _sessionManager;

//        public JoinRoomRequest(RoomsManager roomManager, SessionManager sessionManager)
//        {
//            _roomManager = roomManager;
//            _sessionManager = sessionManager;
//        }
//        public string ServiceName => "JoinRoom";

//        public object Handle(User user, Dictionary<string, object> details)
//        {
//            Dictionary<string, object> response = new Dictionary<string, object>();



//            return response;
//        }
//    }
//}
