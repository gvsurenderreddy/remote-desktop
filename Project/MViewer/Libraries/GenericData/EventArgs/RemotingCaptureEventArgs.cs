using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public class RemotingCaptureEventArgs : EventArgs
    {
        string _identity;

        public string Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }
        byte[] _capture;

        public byte[] Capture
        {
            get { return _capture; }
            set { _capture = value; }
        }
    }
}
