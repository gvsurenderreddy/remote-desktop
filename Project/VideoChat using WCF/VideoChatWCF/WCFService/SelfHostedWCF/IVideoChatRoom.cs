using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace SelfHostedWCF
{
    [ServiceContract]
    public interface IVideoChatRoom
    {
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        //      UriTemplate = "Employees/{Employee_Id}")]
        [OperationContract]
        void SendWebcamCapture(byte[] capture);

        [OperationContract]
        void SendMicrophoneCapture(byte[] capture);

        [OperationContract]
        void InitializeForm();
    }
}
