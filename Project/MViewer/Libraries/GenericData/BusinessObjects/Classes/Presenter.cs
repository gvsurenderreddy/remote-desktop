using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using System.Threading;
using Utils;

namespace GenericObjects
{
    public class Presenter : IPresenter
    {
        #region private members

        WebcamCapture _videoCapture;
        ScreenCaptureTool _screenCapture;
        bool _firstTimeCapturing;
        AudioStreamManager _audioStreamManager;
        PresenterSettings _presenterSettings;

        #endregion

        #region c-tor

        public Presenter(PresenterSettings presenterSettings)
        {
            _presenterSettings = presenterSettings;
            _videoCapture = _presenterSettings.VideoCaptureControl;
            _audioStreamManager = new AudioStreamManager(_presenterSettings.OnAudioCaptureAvailable);

            // initialize the image capture size
            if (_videoCapture != null)
            {
                _videoCapture.ImageHeight = presenterSettings.VideoScreenSize.Height;
                _videoCapture.ImageWidth = presenterSettings.VideoScreenSize.Width;

                _firstTimeCapturing = true;

                // bind the image captured event
                _videoCapture.ImageCaptured += new Delegates.WebCamEventHandler(presenterSettings.OnVideoImageCaptured);
            }

            _screenCapture = new ScreenCaptureTool(_presenterSettings.RemotingTimerInterval, _presenterSettings.OnRemotingImageCaptured);
        }

        #endregion

        #region IPresenter Members

        public void StartVideoPresentation()
        {
            // start the video capturing
            _videoCapture.StartCapturing(_firstTimeCapturing);
            _firstTimeCapturing = false;
        }

        public void StopVideoPresentation()
        {
            if (_videoCapture != null)
            {
                _videoCapture.StopCapturing();
            }
        }

        public void StartAudioPresentation()
        {
            _audioStreamManager.StartStreaming();
        }

        public void StopAudioPresentation()
        {
            _audioStreamManager.StopStreaming();
        }

        public void StartRemotingPresentation()
        {
            _screenCapture.TogglerTimer(true);
        }

        public void StopRemotingPresentation()
        {
            _screenCapture.TogglerTimer(false);
        }

        #endregion

        #region proprieties

        public bool RemotingCaptureClosed()
        {
            return _screenCapture.RemotingCaptureClosed;
        }

        #endregion
    }
}
