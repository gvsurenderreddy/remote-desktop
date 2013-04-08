using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;
using System.IO;

namespace GenericObjects
{
    public interface IClientController
    {
        void StartClient(string identity);
        void RemoveClient(string identity);
        void AddClient(string identity);
        IMViewerService GetClient(string identity);
        bool IsContactOnline(string identity);
        void SendAudioCapture(byte[] capture, string receiverIdentity, string senderIdentity);
        void SendVideoCapture(byte[] capture, string receiverIdentity, string senderIdentity);
        void SendRemotingCapture(byte[] screenCapture, byte[] mouseCapture, string receiverIdentity, string senderIdentity);
        void SendRoomCommand(string myIdentity, string identity, GenericEnums.RoomType roomType, GenericEnums.SignalType signalType);
        void UpdateContactStatus(string partnerIdentity, string myIdentity, GenericEnums.ContactStatus newStatus);
        void UpdateFriendlyName(string partnerIdentity, string myIdentity, string newFriendlyName);
        void WaitRoomButtonAction(string partnerIdentity, string myIdentity, GenericEnums.RoomType roomType, bool wait);
        void SendFile(byte[] fileBytes, string partnerIdentity, string fileName);
        bool SendingPermission(string fileName, long fileSize, string partnerIdentity, string myIdentity);
        void SendRemotingCommand(string receiverIdentity, EventArgs commandArgs);
    }
}
