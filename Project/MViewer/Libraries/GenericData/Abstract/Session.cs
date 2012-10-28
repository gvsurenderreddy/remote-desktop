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
        //protected ConnectedView _view;
        protected GenericEnums.SessionType _sessionType;
        private GenericEnums.SessionState _sessionState;

        #endregion

        #region proprieties

        //public ConnectedView View
        //{
        //    get { return _view; }
        //    set
        //    {
        //        _view.AudioChatView = value.AudioChatView;
        //        _view.VideoChatView = value.VideoChatView;
        //        _view.RemotingView = value.RemotingView;
        //    }
        //}

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

        public GenericEnums.SessionState SessionState
        {
            get { return _sessionState; }
            set { _sessionState = value; }
        }

        #endregion
    }
}
