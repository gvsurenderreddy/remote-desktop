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

        public ClientSession(string identity, GenericEnums.RoomType roomType)
        {
            _identity = identity;
            _peers = new PeerStates();

            switch (roomType)
            {
                case GenericEnums.RoomType.Audio:
                    AudioSessionState = GenericEnums.SessionState.Pending;
                    break;
                case GenericEnums.RoomType.Video:
                    AudioSessionState = GenericEnums.SessionState.Pending;
                    VideoSessionState = GenericEnums.SessionState.Pending;
                    break;
                case GenericEnums.RoomType.Remoting:
                    RemotingSessionState = GenericEnums.SessionState.Pending;
                    break;
            }

            _pendingTransfer = new PendingTransfer();
            _pendingTransfer.Audio = false;
            _pendingTransfer.Video = false;
            _pendingTransfer.Remoting = false;
            _transferUpdating = new TransferStatusUptading();
            _transferUpdating.IsAudioUpdating = false;
            _transferUpdating.IsVideoUpdating = false;
            _transferUpdating.IsRemotingUpdating = false;

        }

        #endregion
    }
}
