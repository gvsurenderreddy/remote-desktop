using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public class ClientSession : Session
    {
         #region c-tor

        public ClientSession(string identity)
        {
            _identity = identity;
            _peers.AudioSessionState = GenericEnums.SessionState.Closed;
            _peers.VideoSessionState = GenericEnums.SessionState.Closed; ;
            _peers.RemotingSessionState = GenericEnums.SessionState.Closed; ;
            _pendingTransfer.Audio = false;
            _pendingTransfer.Video = false;
            _pendingTransfer.Remoting = false;
            _transferUpdating.IsAudioUpdating = false;
            _transferUpdating.IsVideoUpdating = false;
            _transferUpdating.IsRemotingUpdating = false;

        }

        #endregion
    }
}
