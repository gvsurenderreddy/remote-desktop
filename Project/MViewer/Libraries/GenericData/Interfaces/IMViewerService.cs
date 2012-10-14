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

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/SendWebcamCapture", ReplyAction = "http://tempuri.org/IVideoChatRoom/SendWebcamCaptureResponse")]
        void SendWebcamCapture(byte[] capture);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/SendMicrophoneCapture", ReplyAction = "http://tempuri.org/IVideoChatRoom/SendMicrophoneCaptureResponse")]
        void SendMicrophoneCapture(byte[] capture);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/Ping", ReplyAction = "http://tempuri.org/IVideoChatRoom/PingResponse")]
        bool Ping();

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/InitializeRoom", ReplyAction = "http://tempuri.org/IVideoChatRoom/InitializeRoomResponse")]
        void InitializeRoom(string identity, GenericEnums.RoomActionType roomType);
        
    }
}
