
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class AudioCaptureEventArgs : EventArgs
    {
        public byte[] Capture { get; set; }
        public double CaptureLengthInSeconds { get; set; }
        public string Identity { get; set; }
        public DateTime CaptureTimestamp
        {
            get;
            set;
        }
    }
}
