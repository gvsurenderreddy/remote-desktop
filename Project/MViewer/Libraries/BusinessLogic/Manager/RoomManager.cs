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

        // todo: add room type check to the below 3 methods

        public bool VideoRoomsLeft()
        {
            return _rooms != null ? _rooms.Where(kv => kv.Value.RoomType == GenericEnums.RoomType.Video).Count() > 0
                ? true : false : false;
        }

        public bool RemotingRoomsLeft()
        {
            return _rooms != null ? _rooms.Where(kv => kv.Value.RoomType == GenericEnums.RoomType.Remoting).Count() > 0
                ? true : false : false;
        }

        public bool AudioRoomsLeft()
        {
            return _rooms != null ? _rooms.Where(kv => kv.Value.RoomType == GenericEnums.RoomType.Audio).Count() > 0
                 ? true : false : false;
        }

        public bool IsRoomActivated(string identity, GenericEnums.RoomType roomType)
        {
            bool activated = false;
            lock (_syncRooms)
            {
                bool roomExists = _rooms.ContainsKey(identity);
                if (roomExists)
                {
                    activated = _rooms[identity].RoomType == roomType;
                }
            }
            return activated;
        }

        public void PlayAudioCapture(string identity, byte[] capture)
        {
            lock (_syncRooms)
            {
                if (_rooms != null && _rooms.ContainsKey(identity))
                {
                    IAudioRoom room = (IAudioRoom)_rooms[identity];
                    room.PlayAudioCapture(capture);
                }
            }
        }

        public void ShowRemotingCapture(string identity, byte[] screenCapture, byte[] mouseCapture)
        {
            lock (_syncRooms)
            {
                if (_rooms != null && _rooms.ContainsKey(identity))
                {
                    IRemotingRoom room = (IRemotingRoom)_rooms[identity];
                    room.ShowScreenCapture(screenCapture, mouseCapture);
                }
            }
        }

        public void ShowVideoCapture(string identity, Image picture)
        {
            lock (_syncRooms)
            {
                if (_rooms != null && _rooms.ContainsKey(identity))
                {
                    IVideoRoom room = (IVideoRoom)_rooms[identity];
                    room.SetPicture(picture);
                }
            }
        }

        public void SetPartnerName(string identity, string friendlyName)
        {
            lock (_syncRooms)
            {
                if (_rooms != null && _rooms.ContainsKey(identity))
                {
                    IRoom room = _rooms[identity];
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
                if (!_rooms.ContainsKey(identity))
                {
                    _rooms.Add(identity, room);
                }
            }
        }

        public void RemoveRoom(string identity)
        {
            lock (_syncRooms)
            {
                if (_rooms != null && _rooms.ContainsKey(identity))
                {
                    _rooms.Remove(identity);
                }
            }
        }

        public void ShowRoom(string identity)
        {
            //lock (_syncRooms)
            {
                if (_rooms != null && _rooms.ContainsKey(identity))
                {
                    //_mainForm.BeginInvoke((Action)delegate
                    //{
                    //    Form roomForm = (Form)_rooms[identity];
                    //    roomForm.Show();

                    //});
                    Application.Run((Form)_rooms[identity]);
                }
            }
        }

        public void CloseRoom(string identity)
        {
            lock (_syncRooms)
            {
                if (_rooms != null && _rooms.ContainsKey(identity))
                {
                    _rooms[identity].SyncClosing.Reset();
                    ((Form)_rooms[identity]).Invoke(new Delegates.CloseDelegate(((Form)_rooms[identity]).Close));
                }
            }
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
