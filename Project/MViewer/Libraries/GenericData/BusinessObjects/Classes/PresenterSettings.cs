using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////using System.Threading.Tasks;
using Utils;

namespace GenericObjects
{
    public class PresenterSettings
    {
        public WebcamCapture VideoCaptureControl { get; set; }
        public string Identity { get; set; }

        public int VideoTimerInterval { get; set; }
        public Structures.ScreenSize VideoScreenSize { get; set; }
        public EventHandler OnVideoImageCaptured { get; set; }

        public EventHandler OnRemotingImageCaptured { get; set; }
        public int RemotingTimerInterval { get; set; }

        public EventHandler OnAudioCaptureAvailable { get; set; }
    }
}
