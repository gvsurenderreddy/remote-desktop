using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class RemotingCaptureEventArgs : EventArgs
    {
        string _identity;

        public string Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }
        byte[] _screenCapture;

        public byte[] ScreenCapture
        {
            get { return _screenCapture; }
            set { _screenCapture = value; }
        }

        byte[] _mouseCapture;

        public byte[] MouseCapture
        {
            get { return _mouseCapture; }
            set { _mouseCapture = value; }
        }
    }
}
