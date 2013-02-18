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
        void SendCapture(byte[] capture, string receiverIdentity, string senderIdentity);
        void SendRoomCommand(string myIdentity, string identity, GenericEnums.RoomActionType roomType, GenericEnums.SignalType signalType);
        void UpdateContactStatus(string partnerIdentity, string myIdentity, GenericEnums.ContactStatus newStatus);
    }
}
