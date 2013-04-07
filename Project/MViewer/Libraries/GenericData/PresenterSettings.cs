using Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericObjects
{
    public class PresenterSettings : PresenterSettingsBase
    {
        public override IWebcamCapture VideoCaptureControl { get; set; }
        public override string Identity { get; set; }

        public override int VideoTimerInterval { get; set; }
        public override Structures.ScreenSize VideoScreenSize { get; set; }
        public override EventHandler OnVideoImageCaptured { get; set; }

        public override EventHandler OnRemotingImageCaptured { get; set; }
        public override int RemotingTimerInterval { get; set; }

        public override EventHandler OnAudioCaptureAvailable { get; set; }
        public override int AudioTimerInterval { get; set; }
    }
}
