using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;

namespace GenericDataLayer
{
    public interface IRoomManager
    {
        void AddRoom(string identity, IRoom room);
        void RemoveRoom(string identity);
        void ShowRoom(string identity);
        void CloseRoom(string identity);

        void ShowPicture(string identity, Image picture);
        void SetPartnerName(string identity, string friendlyName);
        bool IsRoomActivated(string identity, GenericEnums.RoomActionType roomType);

        string ActiveRoom
        {
            get;
            set;
        }
    }
}
