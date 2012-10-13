using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GenericDataLayer
{
    public class RoomManager : IRoomManager
    {
        #region private members

        Dictionary<string, IRoom> _rooms;
        string _activeRoom;

        #endregion

        #region c-tor

        public RoomManager()
        {
            _rooms = new Dictionary<string, IRoom>();
        }

        #endregion

        #region public methods

        public void ShowPicture(string identity, Image picture)
        {
            if (_rooms.ContainsKey(identity))
            {
                IVideoRoom room = (IVideoRoom)_rooms[identity];
                room.SetPicture(picture);
            }
        }

        public void AddRoom(string identity, IRoom room)
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

        public void RemoveRoom(string identity)
        {
            if (_rooms != null && _rooms.ContainsKey(identity))
            {
                _rooms[identity].CloseRoom();
                _rooms.Remove(identity);
            }
        }

        public void ShowRoom(string identity)
        {
            if (_rooms != null && _rooms.ContainsKey(identity))
            {
                _rooms[identity].ShowRoom();
            }
        }

        public void CloseRoom(string identity)
        {
            if (_rooms != null && _rooms.ContainsKey(identity))
            {
                _rooms[identity].CloseRoom();
            }
        }

        #endregion

        #region proprieties

        public string ActiveRoom
        {
            get { return _activeRoom; }
            set { _activeRoom = value; }
        }

        #endregion
    }
}
