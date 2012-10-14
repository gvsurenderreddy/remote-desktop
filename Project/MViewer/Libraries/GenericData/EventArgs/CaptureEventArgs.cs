using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GenericDataLayer
{
    public class VideoCaptureEventArgs : EventArgs
    {
        #region private members

        private Image _capturedImage;
        //private ulong _frameNumber;
        string _identity;

        #endregion

        #region c-tor

        public VideoCaptureEventArgs()
        {
            //_frameNumber = 0;
        }

        #endregion

        #region proprieties

        /// <summary>
        /// image returned by the web camera capturing device
        /// </summary>
        public Image CapturedImage
        {
            get
            { 
                return _capturedImage; 
            }
            set
            {
                _capturedImage = value; 
            }
        }

        ///// <summary>
        ///// the sequence number of the frame capture
        ///// </summary>
        //public ulong FrameNumber
        //{
        //    get
        //    {
        //        return _frameNumber; 
        //    }
        //    set
        //    {
        //        _frameNumber = value; 
        //    }
        //}

        public string Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }

        #endregion
    }
}
