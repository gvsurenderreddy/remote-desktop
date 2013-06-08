using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class ConferenceStatus
    {
        public bool IsVideoStatusUpdating
        {
            get;
            set;
        }

        public bool IsAudioStatusUpdating
        {
            get;
            set;
        }

        public bool IsRemotingStatusUpdating
        {
            get;
            set;
        }
    }
}
