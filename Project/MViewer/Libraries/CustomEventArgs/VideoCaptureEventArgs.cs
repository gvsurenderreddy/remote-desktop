using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GenericObjects
{
    public class VideoCaptureEventArgs : EventArgs
    {
        #region private members

        private byte[] _capturedImage;
        string _identity;
        DateTime _captureTimestamp;

        #endregion

        #region c-tor


        #endregion

        #region proprieties

        /// <summary>
        /// image returned by the web camera capturing device
        /// </summary>
        public byte[] CapturedImage
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

        public string Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }

        public DateTime CaptureTimestamp
        {
            get { return _captureTimestamp; }
            set { _captureTimestamp = value; }
        }

        #endregion
    }
}
