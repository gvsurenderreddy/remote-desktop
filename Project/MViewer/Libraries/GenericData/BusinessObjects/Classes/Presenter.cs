using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using System.Threading;
using Utils;

namespace GenericDataLayer
{
    public class Presenter : IPresenter
    {
        #region private members

        WebcamCapture _videoCapture;
        ScreenCaptureTool _screenCapture;
        static bool _firstTimeCapturing;

        PresenterSettings _presenterSettings;

        #endregion

        #region c-tor

        public Presenter(PresenterSettings presenterSettings)
        {
            _presenterSettings = presenterSettings;
            _videoCapture = _presenterSettings.VideoCaptureControl;

            // initialize the image capture size
            if (_videoCapture != null)
            {
                _videoCapture.ImageHeight = presenterSettings.VideoScreenSize.Height;
                _videoCapture.ImageWidth = presenterSettings.VideoScreenSize.Width;

                _firstTimeCapturing = true;

                // bind the image captured event
                _videoCapture.ImageCaptured += new Delegates.WebCamEventHandler(presenterSettings.VideoImageCaptured);
            }

            _screenCapture = new ScreenCaptureTool(_presenterSettings.RemotingTimerInterval, _presenterSettings.RemotingImageCaptured);
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
            // todo: implement StartAudioPresentation
        }

        public void StopAudioPresentation()
        {
            // todo: implement StopAudioPresentation
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
