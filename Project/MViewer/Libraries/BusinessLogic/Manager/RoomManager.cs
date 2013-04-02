using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GenericObjects;
using Utils;
using System.Windows.Forms;
using System.Threading;

namespace BusinessLogicLayer
{
    public class RoomManager : IRoomManager
    {
        #region private members

        readonly object _syncRooms = new object();
        IDictionary<string, IRoom> _rooms;

        string _activeRoom;
        Form _mainForm;

        #endregion

        #region c-tor

        public RoomManager(Form mainForm)
        {
            _mainForm = mainForm;
            _rooms = new Dictionary<string, IRoom>();
        }

        #endregion

        #region public methods

        public void ChangeLanguage(string language)
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

        public void ToggleAudioStatus(string identity)
        {
            string roomID = GenerateRoomID(identity, GenericEnums.RoomType.Audio);
            if (_rooms != null && _rooms.ContainsKey(roomID))
            {
                ((IAudioRoom)_rooms[roomID]).ToggleAudioStatus();
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
            lock (_syncRooms)
            {
                string roomID = GenerateRoomID(identity, roomType);
                bool roomExists = _rooms.ContainsKey(roomID);
                if (roomExists)
                {
                    activated = _rooms[roomID].RoomType == roomType;
                }
            }
            return activated;
        }

        public void PlayAudioCapture(string identity, byte[] capture)
        {
            lock (_syncRooms)
            {
                string roomID = GenerateRoomID(identity, GenericEnums.RoomType.Audio);
                if (_rooms != null && _rooms.ContainsKey(roomID))
                {
                    IAudioRoom room = (IAudioRoom)_rooms[roomID];
                    room.PlayAudioCapture(capture);
                }
            }
        }

        public void ShowRemotingCapture(string identity, byte[] screenCapture, byte[] mouseCapture)
        {
            lock (_syncRooms)
            {
                string roomID = GenerateRoomID(identity, GenericEnums.RoomType.Remoting);
                if (_rooms != null && _rooms.ContainsKey(roomID))
                {
                    IRemotingRoom room = (IRemotingRoom)_rooms[roomID];
                    room.ShowScreenCapture(screenCapture, mouseCapture);
                }
            }
        }

        public void ShowVideoCapture(string identity, Image picture)
        {
            lock (_syncRooms)
            {
                string roomID = GenerateRoomID(identity, GenericEnums.RoomType.Video);
                if (_rooms != null && _rooms.ContainsKey(roomID))
                {
                    IVideoRoom room = (IVideoRoom)_rooms[roomID];
                    room.SetPicture(picture);
                }
            }
        }

        public void SetPartnerName(string identity, GenericEnums.RoomType roomType, string friendlyName)
        {
            lock (_syncRooms)
            {
                string roomID = GenerateRoomID(identity, roomType);
                if (_rooms != null && _rooms.ContainsKey(roomID))
                {
                    IRoom room = _rooms[roomID];
                    room.SetPartnerName(friendlyName);
                }
            }
        }

        public void AddRoom(string identity, IRoom room)
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

        public void RemoveRoom(string identity, GenericEnums.RoomType roomType)
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

        public void ShowRoom(string identity, GenericEnums.RoomType roomType)
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
                // todo: log exception
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

        public string ActiveRoom
        {
            get
            {
                lock (_syncRooms)
                {
                    //if (VideoRoomsLeft() == false || RemotingRoomsLeft() == false == AudioRoomsLeft() == false)
                    //{
                    //    // reset the active room identity if the rooms were all closed
                    //    _activeRoom = string.Empty;
                    //}
                    return _activeRoom; 
                }
            }
            set
            {
                lock (_syncRooms)
                { 
                    _activeRoom = value; 
                }
            }
        }

        #endregion
    }
}
