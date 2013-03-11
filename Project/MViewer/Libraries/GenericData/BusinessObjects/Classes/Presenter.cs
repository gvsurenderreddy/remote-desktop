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

        string _identity;
        WebcamCapture _webcamCapture;
        int _timerInterval;
        Structures.ScreenSize _videoSize;
        static bool _firstTimeCapturing;

        #endregion

        #region c-tor

        public Presenter(PresenterSettings presenterSettings)
        {
            _identity = presenterSettings.identity;
            _timerInterval = presenterSettings.timerInterval;
            _videoSize = presenterSettings.videoSize;
            // initialize the webcam capture obj
            _webcamCapture = presenterSettings.captureControl;
            // initialize the image capture size
            if (_webcamCapture != null)
            {
                _webcamCapture.ImageHeight = _videoSize.Height;
                _webcamCapture.ImageWidth = _videoSize.Width;

                _firstTimeCapturing = true;

                // bind the image captured event
                _webcamCapture.ImageCaptured += new WebcamCapture.WebCamEventHandler(presenterSettings.webCamImageCaptured);
            }
        }

        #endregion

        #region IPresenter Members

        public void StartVideoPresentation()
        {
            // start the video capturing
            _webcamCapture.StartCapturing(_firstTimeCapturing);
            _firstTimeCapturing = false;
        }

        public void StopVideoPresentation()
        {
            if (_webcamCapture != null)
            {
                _webcamCapture.StopCapturing();
            }
        }

        public void StartAudioPresentation()
        {
            throw new NotImplementedException();
        }

        public void StopAudioPresentation()
        {
            throw new NotImplementedException();
        }

        public void StartRemotingPresentation()
        {
            throw new NotImplementedException();
        }

        public void StopRemotingPresentation()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
