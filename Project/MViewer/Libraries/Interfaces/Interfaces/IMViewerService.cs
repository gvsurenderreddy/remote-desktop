using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Drawing;
using System.IO;

namespace GenericObjects
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IVideoRoom")]
    public interface IMViewerService
    {
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/SendingPermission", ReplyAction = "http://tempuri.org/IVideoRoom/SendingPermissionResponse")]
        bool SendingPermission(string senderIdentity, string fileName, long fileSize);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/ConferencePermission", ReplyAction = "http://tempuri.org/IVideoRoom/ConferencePermissionResponse")]
        bool ConferencePermission(string senderIdentity, GenericEnums.RoomType roomType);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/UpdateContactStatus", ReplyAction = "http://tempuri.org/IVideoRoom/UpdateContactStatusResponse")]
        void UpdateContactStatus(string senderIdentity, GenericEnums.ContactStatus newStatus);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/UpdateFriendlyName", ReplyAction = "http://tempuri.org/IVideoRoom/UpdateFriendlyNameResponse")]
        void UpdateFriendlyName(string senderIdentity, string newFriendlyName);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/SendWebcamCapture", ReplyAction = "http://tempuri.org/IVideoRoom/SendWebcamCaptureResponse")]
        void SendWebcamCapture(byte[] capture, DateTime captureTimestamp, string senderIdentity);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/SendRemotingCapture", ReplyAction = "http://tempuri.org/IVideoRoom/SendRemotingCaptureResponse")]
        void SendRemotingCapture(byte[] screenCapture, byte[] mouseCapture, string senderIdentity);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/SendMicrophoneCapture", ReplyAction = "http://tempuri.org/IVideoRoom/SendMicrophoneCaptureResponse")]
        void SendMicrophoneCapture(byte[] capture, DateTime captureTimestamp, string senderIdentity);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/Ping", ReplyAction = "http://tempuri.org/IVideoRoom/PingResponse")]
        bool Ping();

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/SendRoomButtonAction", ReplyAction = "http://tempuri.org/IVideoRoom/SendRoomButtonActionResponse")]
        void SendRoomButtonAction(string identity, GenericEnums.RoomType roomType, GenericEnums.SignalType signalType);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/WaitRoomButtonAction", ReplyAction = "http://tempuri.org/IVideoRoom/WaitRoomButtonActionResponse")]
        void WaitRoomButtonAction(string senderIdentity, GenericEnums.RoomType roomType, bool wait);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/AddContact", ReplyAction = "http://tempuri.org/IVideoRoom/AddContactResponse")]
        void AddContact(string identity, string friendlyName);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/RemoveContact", ReplyAction = "http://tempuri.org/IVideoRoom/RemoveContactResponse")]
        void RemoveContact(string identity);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/SendFile", ReplyAction = "http://tempuri.org/IVideoRoom/SendFileResponse")]
        void SendFile(byte[] fileBytes, string fileName, string senderIdentity);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoRoom/SendRemotingCommand", ReplyAction = "http://tempuri.org/IVideoRoom/SendRemotingCommandResponse")]
        void SendRemotingCommand(byte[] commandArgs);

    }
}
