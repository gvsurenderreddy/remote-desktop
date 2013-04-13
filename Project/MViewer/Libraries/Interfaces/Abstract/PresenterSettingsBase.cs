using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Abstraction
{
    public abstract class PresenterSettingsBase
    {
        protected IWebcamCapture videoCaptureControl;
        protected string identity;

        protected int videoTimerInterval;
        protected DescriptorUtils.Structures.ScreenSize videoScreenSize;
        protected EventHandler onVideoImageCaptured;

        protected EventHandler onRemotingImageCaptured;
        protected int remotingTimerInterval;

        protected EventHandler onAudioCaptureAvailable;
        protected int audioTimerInterval;
    }
}
