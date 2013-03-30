using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;

namespace GenericObjects
{
    public interface IRoomManager
    {
        void AddRoom(string identity, IRoom room);
        void RemoveRoom(string identity);
        void ShowRoom(string identity);
        void CloseRoom(string identity);

        void PlayAudioCapture(string identity, byte[] capture);
        void ShowVideoCapture(string identity, Image picture);
        void ShowRemotingCapture(string identity, byte[] screenCapture, byte[] mouseCapture);
        void SetPartnerName(string identity, string friendlyName);
        bool IsRoomActivated(string identity, GenericEnums.RoomType roomType);
 
        bool VideoRoomsLeft();
        bool RemotingRoomsLeft();
        bool AudioRoomsLeft();

        string ActiveRoom
        {
            get;
            set;
        }
    }
}
