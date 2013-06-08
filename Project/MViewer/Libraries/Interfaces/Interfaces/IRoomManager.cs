using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;
using Abstraction;

namespace GenericObjects
{
    public interface IRoomManager
    {
        void ChangeLanguage(string language);
        void AddRoom(string identity, IRoom room);
        void RemoveRoom(string identity, GenericEnums.RoomType roomType);
        void ShowRoom(string identity, GenericEnums.RoomType roomType);
        void CloseRoom(string identity, GenericEnums.RoomType roomType);

        void PlayAudioCapture(string identity, byte[] capture, DateTime timestamp, double captureLengthInSeconds);
        void ShowVideoCapture(string identity, Image picture, DateTime timestamp);
        void ShowRemotingCapture(string identity, byte[] screenCapture, byte[] mouseCapture);
        void SetPartnerName(string identity, GenericEnums.RoomType roomType, string friendlyName);
        bool IsRoomActivated(string identity, GenericEnums.RoomType roomType);
        void ToggleAudioStatus(string identity);

        bool RoomsLeft(GenericEnums.RoomType roomType);

        ActiveRoomsBase ActiveRooms
        {
            get;
            set;
        }
    }
}
