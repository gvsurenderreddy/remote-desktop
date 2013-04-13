
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
        public IWebcamCapture VideoCaptureControl { get; set; }
        public string Identity { get; set; }

        public int VideoTimerInterval { get; set; }
        public DescriptorUtils.Structures.ScreenSize VideoScreenSize { get; set; }
        public EventHandler OnVideoImageCaptured { get; set; }

        public EventHandler OnRemotingImageCaptured { get; set; }
        public int RemotingTimerInterval { get; set; }

        public EventHandler OnAudioCaptureAvailable { get; set; }
        public int AudioTimerInterval { get; set; }
    }
}
