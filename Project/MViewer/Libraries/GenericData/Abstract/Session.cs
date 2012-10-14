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
        protected ConnectedPeers _peers;
        protected GenericEnums.SessionType _sessionType;

        #endregion

        #region proprieties

        IRoom AudioChatView
        {
            get;
            set;
        }
        
        IRoom VideoChatView
        {
            get;
            set;
        }

        IRoom RemoteView
        {
            get;
            set;
        }

        public string Identity
        {
            get { return _identity; }
        }

        public ConnectedPeers Peers
        {
            get { return _peers; }
            set
            {
                _peers.Audio = value.Audio;
                _peers.Video = value.Video;
                _peers.Remoting = value.Remoting;
            }
        }

        public GenericEnums.SessionType SessionType
        {
            get { return _sessionType; }
        }

        #endregion
    }
}
