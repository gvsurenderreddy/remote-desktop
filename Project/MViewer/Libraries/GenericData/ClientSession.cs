using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericObjects
{
    public class ClientSession : Session
    {
         #region c-tor

        public ClientSession(string identity, GenericEnums.RoomType roomType)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region proprieties

        public string Identity
        {
            get { return _identity; }
        }

        public PendingTransfer PendingTransfer
        {
            get { return _pendingTransfer; }
            set { _pendingTransfer = value; }
        }

        public TransferStatusUptading TransferUpdating
        {
            get { return _transferUpdating; }
            set { _transferUpdating = value; }
        }

        public PeerStates Peers
        {
            get
            {
                return _peers;
            }
        }

        public GenericEnums.SessionState VideoSessionState
        {
            get { return _peers.VideoSessionState; }
            set { _peers.VideoSessionState = value; }
        }

        public GenericEnums.SessionState AudioSessionState
        {
            get { return _peers.AudioSessionState; }
            set { _peers.AudioSessionState = value; }
        }

        public GenericEnums.SessionState RemotingSessionState
        {
            get { return _peers.RemotingSessionState; }
            set { _peers.RemotingSessionState = value; }
        }

        #endregion
    }
}
