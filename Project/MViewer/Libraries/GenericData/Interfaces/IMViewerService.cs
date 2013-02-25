using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Drawing;

namespace GenericDataLayer
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IVideoChatRoom")]
    public interface IMViewerService
    {
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/UpdateContactStatus", ReplyAction = "http://tempuri.org/IVideoChatRoom/UpdateContactStatusResponse")]
        void UpdateContactStatus(string senderIdentity, GenericEnums.ContactStatus newStatus);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/UpdateFriendlyName", ReplyAction = "http://tempuri.org/IVideoChatRoom/UpdateFriendlyNameResponse")]
        void UpdateFriendlyName(string senderIdentity, string newFriendlyName);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/SendWebcamCapture", ReplyAction = "http://tempuri.org/IVideoChatRoom/SendWebcamCaptureResponse")]
        void SendWebcamCapture(byte[] capture, string senderIdentity);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/SendMicrophoneCapture", ReplyAction = "http://tempuri.org/IVideoChatRoom/SendMicrophoneCaptureResponse")]
        void SendMicrophoneCapture(byte[] capture);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/Ping", ReplyAction = "http://tempuri.org/IVideoChatRoom/PingResponse")]
        bool Ping();

        //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/InitializeRoom", ReplyAction = "http://tempuri.org/IVideoChatRoom/InitializeRoomResponse")]
        //void InitializeRoom(string identity, GenericEnums.RoomActionType roomType);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/SendRoomAction", ReplyAction = "http://tempuri.org/IVideoChatRoom/SendRoomActionResponse")]
        void SendRoomAction(string identity, GenericEnums.RoomActionType roomType, GenericEnums.SignalType signalType);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/AddContact", ReplyAction = "http://tempuri.org/IVideoChatRoom/AddContactResponse")]
        void AddContact(string identity, string friendlyName);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/RemoveContact", ReplyAction = "http://tempuri.org/IVideoChatRoom/RemoveContactResponse")]
        void RemoveContact(string identity);

        //[System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/CloseRoom", ReplyAction = "http://tempuri.org/IVideoChatRoom/CloseRoomResponse")]
        //void CloseRoom(string identity, GenericEnums.RoomActionType roomType);


    }
}
