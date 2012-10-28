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
        static IPresenter _presenter;

        #endregion

        #region c-tor

        static PresenterManager()
        {

        }

        #endregion

        #region public methods

        //public void AddPresenter(string identity, IPresenter presenter)
        //{
        //    if (_presenters == null)
        //    {
        //        _presenters = new Dictionary<string, IPresenter>();
        //    }
        //    if (!_presenters.ContainsKey(identity))
        //    {
        //        _presenters.Add(identity, presenter);
        //    }
        //}

        //public void RemovePresenter(string identity)
        //{
        //    if (_presenters != null && _presenters.ContainsKey(identity))
        //    {
        //        _presenters[identity].StopPresentation();
        //        _presenters.Remove(identity);
        //    }
        //}

        public static void StartPresentation(WebcamCapture webcapture, string identity, int timerInterval, int height, int width, EventHandler webCamImageCaptured)
        {
            if (_presenter == null)
            {
                lock (_syncPresenter)
                {
                    if (_presenter == null)
                    {
                        _presenter = new Presenter(webcapture, identity, timerInterval, height, width, webCamImageCaptured);
                    }
                }
            }
            _presenter.StartPresentation();
        }

        public static void StopPresentation()
        {
            lock (_syncPresenter)
            {
                if (_presenter != null)
                {
                    _presenter.StopPresentation();
                }
            }
        }

        #endregion
    }
}
