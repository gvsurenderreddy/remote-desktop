using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;

namespace BusinessLogicLayer
{
    public static class PresenterManager 
    {
        #region private members

        static readonly object _syncPresenter = new object();
        static Presenter _presenter;

        #endregion

        #region c-tor

        static PresenterManager()
        {

        }

        #endregion

        #region public methods

        public static Presenter Instance(WebcamCapture captureControl, string identity, int timerInterval, int height, int width, EventHandler webCamImageCaptured)
        {
            if (_presenter == null)
            {
                lock (_syncPresenter)
                {
                    if (_presenter == null)
                    {
                        _presenter = new Presenter(captureControl, identity, timerInterval, height, width, webCamImageCaptured);
                    }
                }
            }
            return _presenter;
        }

        //public void StartPresentation(WebcamCapture webcapture, string identity, int timerInterval, int height, int width, EventHandler webCamImageCaptured)
        //{
        //    _presenter.StartPresentation();
        //}

        //public void StopPresentation()
        //{
        //    lock (_syncPresenter)
        //    {
        //        if (_presenter != null)
        //        {
        //            _presenter.StopPresentation();
        //        }
        //    }
        //}

        #endregion
    }
}
