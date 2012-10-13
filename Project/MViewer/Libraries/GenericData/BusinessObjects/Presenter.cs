using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using System.Threading;

namespace GenericDataLayer
{
    public class Presenter : IPresenter
    {
        #region private members

        string _identity;
        WebcamCapture _webcamCapture;
        int _timerInterval;
        int _height;
        int _width;

        #endregion

        #region c-tor

        public Presenter(WebcamCapture captureControl, string identity, int timerInterval, int height, int width, EventHandler webCamImageCaptured)
        {
            _identity = identity;
            _timerInterval = timerInterval;
            _width = width;
            _height = height;
            // initialize the webcam capture obj
            _webcamCapture = captureControl;
            // initialize the image capture size
            _webcamCapture.ImageHeight = _height;
            _webcamCapture.ImageWidth = _width;

            // bind the image captured event
            _webcamCapture.ImageCaptured += new WebcamCapture.WebCamEventHandler(webCamImageCaptured);
        }

        #endregion

        #region public methods

        public void StartPresentation()
        {
            // start the video capturing
            _webcamCapture.StartCapturing();
        }

        public void StopPresentation()
        {

        }

        #endregion

        #region private methods

        #endregion
    }
}
