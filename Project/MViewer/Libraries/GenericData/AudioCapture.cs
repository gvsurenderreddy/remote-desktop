using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class AudioCapture
    {
        public byte[] Capture { get; set; }
        public DateTime ReceiveTimestamp { get; set; }
    }
}
