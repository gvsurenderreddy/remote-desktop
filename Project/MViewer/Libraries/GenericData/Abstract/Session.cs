using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace GenericDataLayer
{
    public abstract class Session
    {
        #region protected members

        protected string _identity;
        protected PeerStates _peers;
        protected PendingTransfer _pendingTransfer;
        protected TransferStatusUptading _transferUpdating;

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
