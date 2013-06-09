using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GenericObjects;
using Utils;
using System.Windows.Forms;
using System.Threading;
using Abstraction;

namespace BusinessLogicLayer
{
    public class RoomManager : IRoomManager
    {
        #region private members

        readonly object _syncRooms = new object();
        IDictionary<string, IRoom> _rooms;

        ActiveRoomsBase _activeRooms;
        Form _mainForm;

        #endregion

        #region c-tor

        public RoomManager(Form mainForm)
        {
            _mainForm = mainForm;
            _rooms = new Dictionary<string, IRoom>();
            ActiveRooms = new ActiveRooms()
            {
                AudioRoomIdentity = string.Empty,
                VideoRoomIdentity = string.Empty,
                RemotingRoomIdentity = string.Empty
            };
        }

        #endregion

        #region public methods

        public void ChangeLanguage(string language)
        {
            try
            {
                foreach (KeyValuePair<string, IRoom> room in _rooms)
                {
                    Thread t = new Thread(delegate()
                    {
                        room.Value.ChangeLanguage(language);
                    });
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void ToggleAudioStatus(string identity)
        {
            try
            {
                string roomID = GenerateRoomID(identity, GenericEnums.RoomType.Audio);
                if (_rooms != null && _rooms.ContainsKey(roomID))
                {
                    ((IAudioRoom)_rooms[roomID]).ToggleAudioStatus();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public bool RoomsLeft(GenericEnums.RoomType roomType)
        {
            return _rooms != null ? _rooms.Where(kv => kv.Value.RoomType == roomType).Count() > 0
                ? true : false : false;
        }

        public bool IsRoomActivated(string identity, GenericEnums.RoomType roomType)
        {
            bool activated = false;
            try
            {
                lock (_syncRooms)
                {
                    string roomID = GenerateRoomID(identity, roomType);
                    bool roomExists = _rooms.ContainsKey(roomID);
                    if (roomExists)
                    {
                        activated = _rooms[roomID].RoomType == roomType;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return activated;
        }

        public void PlayAudioCapture(string identity, byte[] capture, DateTime timestamp, double captureLengthInSeconds)
        {
            try
            {
                //lock (_syncRooms)
                {
                    string roomID = GenerateRoomID(identity, GenericEnums.RoomType.Audio);
                    if (_rooms != null && _rooms.ContainsKey(roomID))
                    {
                        IAudioRoom room = (IAudioRoom)_rooms[roomID];
                        room.PlayAudioCapture(capture, captureLengthInSeconds);
                        string videoRoomID = GenerateRoomID(identity, GenericEnums.RoomType.Video);
                        if(_rooms.ContainsKey(videoRoomID))
                        {
                            IVideoRoom videoRoom = (IVideoRoom)_rooms[videoRoomID];
                            videoRoom.LastAudioTimestamp = timestamp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void ShowRemotingCapture(string identity, byte[] screenCapture, byte[] mouseCapture)
        {
            try
            {
                //lock (_syncRooms)
                {
                    string roomID = GenerateRoomID(identity, GenericEnums.RoomType.Remoting);
                    if (_rooms != null && _rooms.ContainsKey(roomID))
                    {
                        IRemotingRoom room = (IRemotingRoom)_rooms[roomID];
                        room.ShowScreenCapture(screenCapture, mouseCapture);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void ShowVideoCapture(string identity, byte[] picture, DateTime timestamp)
        {
            try
            {
                //lock (_syncRooms)
                {
                    string roomID = GenerateRoomID(identity, GenericEnums.RoomType.Video);
                    if (_rooms != null && _rooms.ContainsKey(roomID))
                    {
                        IVideoRoom room = (IVideoRoom)_rooms[roomID];
                        room.SetPicture(picture, timestamp);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SetPartnerName(string identity, GenericEnums.RoomType roomType, string friendlyName)
        {
            try
            {
                //lock (_syncRooms)
                {
                    string roomID = GenerateRoomID(identity, roomType);
                    if (_rooms != null && _rooms.ContainsKey(roomID))
                    {
                        IRoom room = _rooms[roomID];
                        room.SetPartnerName(friendlyName);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void AddRoom(string identity, IRoom room)
        {
            try
            {
                lock (_syncRooms)
                {
                    if (_rooms == null)
                    {
                        _rooms = new Dictionary<string, IRoom>();
                    }
                    string roomID = GenerateRoomID(identity, room.RoomType);
                    if (!_rooms.ContainsKey(roomID))
                    {
                        _rooms.Add(roomID, room);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void RemoveRoom(string identity, GenericEnums.RoomType roomType)
        {
            try
            {
                lock (_syncRooms)
                {
                    string roomID = GenerateRoomID(identity, roomType);
                    if (_rooms != null && _rooms.ContainsKey(roomID))
                    {
                        _rooms.Remove(roomID);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void ShowRoom(string identity, GenericEnums.RoomType roomType)
        {
            try
            {
                //lock (_syncRooms)
                {
                    string roomID = GenerateRoomID(identity, roomType);
                    if (_rooms != null && _rooms.ContainsKey(roomID))
                    {
                        Application.Run((Form)_rooms[roomID]);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void CloseRoom(string identity, GenericEnums.RoomType roomType)
        {
            try
            {
                lock (_syncRooms)
                {
                    string roomID = GenerateRoomID(identity, roomType);
                    if (_rooms != null && _rooms.ContainsKey(roomID))
                    {
                        _rooms[roomID].SyncClosing.Reset();
                        ((Form)_rooms[roomID]).Invoke(new Delegates.CloseDelegate(((Form)_rooms[roomID]).Close));
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        string GenerateRoomID(string partnerID, GenericEnums.RoomType roomType)
        {
            return partnerID + roomType.ToString();
        }

        #endregion

        #region proprieties

        public ActiveRoomsBase ActiveRooms
        {
            get
            {
                //lock (_syncRooms)
                {
                    //if (VideoRoomsLeft() == false || RemotingRoomsLeft() == false == AudioRoomsLeft() == false)
                    //{
                    //    // reset the active room identity if the rooms were all closed
                    //    _activeRoom = string.Empty;
                    //}
                    return _activeRooms; 
                }
            }
            set
            {
                //lock (_syncRooms)
                { 
                    _activeRooms = value; 
                }
            }
        }

        #endregion
    }
}
