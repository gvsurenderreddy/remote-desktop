using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;

namespace GenericDataLayer
{
    public interface IClientController
    {
        void StartClient(string identity);
        void RemoveClient(string identity);
        void AddClient(string identity);
        MViewerClient GetClient(string identity);
        bool IsContactOnline(string identity);
        void SendCapture(byte[]capture, string identity);
        void SendRoomCommand(string identity, GenericEnums.RoomActionType roomType, GenericEnums.SignalType signalType);
    }
}
