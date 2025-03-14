using Newtonsoft.Json;
using System;
using GameServer.Interfaces;
using GameServer.Managers;

namespace GameServer.Models
{
    public class GameRoom
    {
        private string _roomId;
        private RoomsManager _roomManager;
        private IRandomizerService _randomizerService;
        private IDateTimeService _dateTimeService;
        private SessionManager _sessionManager;


        private string _roomName;
        private string _roomOwner;
        private int _maxUsersCount;
       

        private bool _isRoomActive = false;
        private bool _isDestroyThread = false;
        private int _moveCounter = 0;
        private int _turnIndex = 0;
        private int _turnTime = 10;
        private int _timeOutTime = 35;
        private RoomTime _roomTime;

        private Dictionary<string, object> _roomProperties;
        private List<string> _playersOrder;
        private Dictionary<string,User> _subscribedUsers;
        private Dictionary<string, User> _joinedUsers;


        public GameRoom(string roomId, RoomsManager roomManager,
           IRandomizerService randomizerService,IDateTimeService dateTimeService, SessionManager sessionManager,
                string roomName, string roomOwner, int maxUsersCount, Dictionary<string, object> properties)
        {
            _roomName = roomName;
            _roomOwner = roomOwner;
            _maxUsersCount = maxUsersCount;
            _roomId = roomId;
            _roomManager = roomManager;
            _randomizerService = randomizerService;
            _dateTimeService = dateTimeService;
            _sessionManager = sessionManager;
            _isRoomActive = false;
            _moveCounter = 0;
            _turnIndex = 0;
            _roomTime = new RoomTime(_turnTime, _timeOutTime);
            _roomProperties = new Dictionary<string, object>(properties);
            _playersOrder = new List<string>();
            _subscribedUsers = new Dictionary<string, User>();
            _joinedUsers = new Dictionary<string, User>();

        }

        #region Requests
        public Dictionary<string,object> ReceivedMove(User curUser,string boardIndex)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            if (_playersOrder[_turnIndex] == curUser.UserId)
            {
                PassTurn();
                response = new Dictionary<string, object>()
                {
                    {"Service","BroadcastMove" },
                    {"Sender",curUser.UserId},
                    {"RoomId", _roomId},
                    {"MoveData", boardIndex},
                    {"NextTurn",_playersOrder[_turnIndex] }
                };

                string toSend = JsonConvert.SerializeObject(response);
                BroadcastToRoom(toSend);
            }
            else response.Add("ErrorCode", GlobalEnums.ErrorCodes.NotPlayerTurn);

            return response;
        }

        public Dictionary<string, object> StopGame(User user, string winner)
        {
            foreach (string userId in _joinedUsers.Keys)
            {
                User usr = _sessionManager.GetUser(userId);
                if (usr != null)
                {
                    usr.CurUserState = User.UserState.Idle;
                    usr.RoomId = "";
                    _sessionManager.UpdateUser(usr);
                }
            }
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                {"Service","StopGame"},
                {"Winner",winner}
            };

            string toSend = JsonConvert.SerializeObject(response);
            BroadcastToRoom(toSend);

            return response;
        }
        #endregion

        #region GameLoop

        public void GameLoop()
        {
            if(_isRoomActive)
            {
                try
                {
                    if (_roomTime.IsCurrentTimeActive() == false)
                        ChangeTurn();

                    if(_isDestroyThread && _roomTime.IsRoomTimeOut() == false)
                    {
                        Console.WriteLine("Destroying room");
                        CloseRoom();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("GameLoop:Error: " + ex.ToString());   
                    CloseRoom();
                }
            }
        }

        #endregion

        #region Logic
        public void StartGame(string uid)
        {
            foreach (string userId in _joinedUsers.Keys)
            {
                User user = _sessionManager.GetUser(userId);
                if (user != null)
                {
                    user.CurUserState = User.UserState.Playing;
                    user.RoomId = _roomId;
                    _sessionManager.UpdateUser(user);
                }
            }


            _turnIndex = _randomizerService.GetRandomNumber(0, 1);

            Dictionary<string, object> sendData = new Dictionary<string, object>()
            {
                { "Service","StartGame"},
                { "Sender", uid },
                { "RoomId",_roomId},
                { "TurnTime",_turnTime},
                { "NextTurn",_playersOrder[_turnIndex]},
                { "Players",_playersOrder},
                { "TurnsList",new List<object>( GetJoinedUsersList())}
            };

            string toSend = JsonConvert.SerializeObject(sendData);
            BroadcastToRoom(toSend);

            _isRoomActive = true;
            _isDestroyThread = true;
            _roomTime.ResetTimer();
        }

        private void PassTurn()
        {
            _moveCounter++;
            _turnIndex = _turnIndex == 0 ? 1 : 0;
            _roomTime.ResetTimer();
        }

        private void CloseRoom()
        {
            Console.WriteLine("Closed Room " + DateTime.UtcNow.ToShortTimeString());
           _isRoomActive = false;
            _roomManager.RemoveRoom(_roomId);
        }

        private void ChangeTurn()
        {
            PassTurn();
            Dictionary<string, object> notifyData = new Dictionary<string, object>()
            {
                { "Service","PassTurn"},
                { "CP",_playersOrder[_turnIndex]},
                { "MC",_moveCounter}
            }; 
            
            string toSend = JsonConvert.SerializeObject(notifyData);
            BroadcastToRoom(toSend);
        }

        #endregion

        public void BroadcastToRoom(string toSend)
        {
            foreach (string userId in _subscribedUsers.Keys)
                _subscribedUsers[userId].SendMessage(toSend);
        }

        public void SendChat(User user, string message)
        {
            Dictionary<string, object> broadcastData = new Dictionary<string, object>()
            {
                {"Service","SendChat"},
                {"Sender",user.UserId},
                {"RoomId",user.RoomId},
                {"Message",message}
            };

            string toSend = JsonConvert.SerializeObject(broadcastData);
            BroadcastToRoom(toSend);
        }

        public Dictionary<string, object> GetRoomDetails()
        {
            if (_isRoomActive)
            {
                return null;
            }

            Dictionary<string, object> roomData = new Dictionary<string, object>()
            {
                {"RoomId",_roomId },
                {"Name",_roomName},
                {"Owner",_roomOwner},
                {"MaxUsersCount",_maxUsersCount},
                {"JoinedUsersCount",_joinedUsers.Count},
                {"TurnTime", _turnTime },

            };
            return roomData;
        }

        public bool TryAddUser(User user)
        {
            if (user == null || _joinedUsers.Count >= _maxUsersCount || _joinedUsers.ContainsKey(user.UserId))
                return false;

            
            user.RoomId = _roomId;
            _joinedUsers.Add(user.UserId, user);
            _playersOrder.Add(user.UserId);

            Dictionary<string, object> broadcastData = new Dictionary<string, object>()
            {
                {"Service","UserJoinRoom"},
                {"UserId",user.UserId},
                {"RoomData", new Dictionary<string,object>(GetRoomDetails())},

            };
            string toSend = JsonConvert.SerializeObject(broadcastData);
            BroadcastToRoom(toSend);
            return true;
            
        }

        public bool AddSubscriber(User user)
        {
            if (user == null || _subscribedUsers.ContainsKey(user.UserId))
                return false;
            _subscribedUsers.Add(user.UserId, user);
            return true;
        }

        public Dictionary<string,object> GetRoomProperties()
        {
            return _roomProperties;
        }

        public List<object> GetJoinedUsersList ()
        {
            List<object> users = new List<object>();
            foreach (User user in _joinedUsers.Values)
                users.Add(user.UserId);
            return users;
        }

        public bool RemoveUser(User user)
        {
            if (user == null)
                return false;

            if(_subscribedUsers.ContainsKey(user.UserId))
            {
                _subscribedUsers.Remove(user.UserId);
            }

            if (_joinedUsers.ContainsKey(user.UserId))
            {
                _joinedUsers.Remove(user.UserId);
                _playersOrder.Remove(user.UserId);
                user.RoomId = "";
                user.CurUserState = User.UserState.Idle;
                _sessionManager.UpdateUser(user);
               
                if(_roomOwner == user.UserId)
                {
                    if (_joinedUsers.Count > 0)
                    {
                        _roomOwner = _joinedUsers.Keys.First();
                        _roomProperties["Owner"] = _roomOwner;
                    }
                    else
                    {
                        CloseRoom();
                    }
                }
            }
            return true;
        }

    }
}
