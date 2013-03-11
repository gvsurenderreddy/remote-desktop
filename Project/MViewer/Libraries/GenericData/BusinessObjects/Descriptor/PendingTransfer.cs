using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public class PendingTransfer
    {
        public bool Video
        {
            get;
            set;
        }

        public bool Audio
        {
            get;
            set;
        }

        public bool Remoting
        {
            get;
            set;
        }
    }
}
