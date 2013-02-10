using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GenericDataLayer;
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

        public bool RoomsLeft()
        {
            return _rooms != null ? _rooms.Count > 0 ? true : false : false;
        }

        public bool IsRoomActivated(string identity, GenericEnums.RoomActionType roomType)
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

        public void ShowPicture(string identity, Image picture)
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
            Thread t = new Thread(delegate()
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
                );
            t.IsBackground = true;
            t.Start();
        }

        public void RemoveRoom(string identity)
        {
            lock (_syncRooms)
            {
                if (_rooms != null && _rooms.ContainsKey(identity))
                {
                    _rooms[identity].CloseRoom();
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
                    _rooms[identity].CloseRoom();
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
                { return _activeRoom; }
            }
            set
            {
                lock (_syncRooms)
                { _activeRoom = value; }
            }
        }

        #endregion
    }
}
