using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericObjects
{
    public class PeerStates
    {
        GenericEnums.SessionState _videoSessionState;

        public GenericEnums.SessionState VideoSessionState
        {
            get { return _videoSessionState; }
            set { _videoSessionState = value; }
        }

        GenericEnums.SessionState _audioSessionState;

        public GenericEnums.SessionState AudioSessionState
        {
            get { return _audioSessionState; }
            set { _audioSessionState = value; }
        }
        GenericEnums.SessionState _remotingSessionState;

        public GenericEnums.SessionState RemotingSessionState
        {
            get { return _remotingSessionState; }
            set { _remotingSessionState = value; }
        }
    }
}
