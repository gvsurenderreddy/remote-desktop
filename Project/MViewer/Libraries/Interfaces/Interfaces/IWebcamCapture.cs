using GenericObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Abstraction
{
    public interface IWebcamCapture
    {
        void StartCapturing(bool firstTimeCapturing);
        void WaitRoomButtonAction(bool wait);
        void StopCapturing();
        Form ParentForm
        {
            get;
            set ; 
        }
        bool WebcaptureClosed
        {
            get;
        }
        bool ThreadAborted
        {
            get;
        }
        int CaptureTimespan
        {
            set;
        }
        int ImageHeight
        {
            set;
        }
        int ImageWidth
        {
            set;
        }
        event Delegates.WebCamEventHandler ImageCaptured;
    }
}
