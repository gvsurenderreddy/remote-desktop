using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public struct TransferUptading
    {
        public bool IsVideoUpdating
        {
            get;
            set;
        }

        public bool IsAudioUpdating
        {
            get;
            set;
        }

        public bool IsRemotingUpdating
        {
            get;
            set;
        }
    }
}
