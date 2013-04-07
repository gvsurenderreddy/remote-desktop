using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Abstraction
{
    public abstract class PresenterSettingsBase
    {
        public abstract IWebcamCapture VideoCaptureControl { get; set; }
        public abstract string Identity { get; set; }

        public abstract int VideoTimerInterval { get; set; }
        public abstract Structures.ScreenSize VideoScreenSize { get; set; }
        public abstract EventHandler OnVideoImageCaptured { get; set; }

        public abstract EventHandler OnRemotingImageCaptured { get; set; }
        public abstract int RemotingTimerInterval { get; set; }

        public abstract EventHandler OnAudioCaptureAvailable { get; set; }
        public abstract int AudioTimerInterval { get; set; }
    }
}
