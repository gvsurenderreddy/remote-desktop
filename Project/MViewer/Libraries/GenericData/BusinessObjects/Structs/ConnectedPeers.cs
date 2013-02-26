using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public struct PeerStates
    {
        public GenericEnums.SessionState VideoSessionState;
        public GenericEnums.SessionState AudioSessionState;
        public GenericEnums.SessionState RemotingSessionState;
    }
}
