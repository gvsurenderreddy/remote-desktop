using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public struct ConnectedView
    {
        public IRoom AudioChatView
        {
            get;
            set;
        }

        public IRoom VideoChatView
        {
            get;
            set;
        }

        public IRoom RemotingView
        {
            get;
            set;
        }
    }
}
