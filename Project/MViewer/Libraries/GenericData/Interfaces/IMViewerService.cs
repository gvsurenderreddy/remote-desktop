using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/InitializeForm", ReplyAction = "http://tempuri.org/IVideoChatRoom/InitializeFormResponse")]
        void InitializeForm();

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IVideoChatRoom/Ping", ReplyAction = "http://tempuri.org/IVideoChatRoom/PingResponse")]
        bool Ping();

    }
}
