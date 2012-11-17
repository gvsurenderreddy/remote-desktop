using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using Utils;

namespace BusinessLogicLayer
{
    public class SessionManager : ISessionManager
    {
        #region private members

        IDictionary<string, Session> _clientSessions;
        readonly object _syncSessions = new object();

        #endregion

        #region public methods

        public void AddSession(Session session)
        {
            lock (_syncSessions)
            {
                switch (session.SessionType)
                {
                    case GenericEnums.SessionType.ClientSession:
                        if (_clientSessions == null)
                        {
                            _clientSessions = new Dictionary<string, Session>();
                        }
                        if (_clientSessions.ContainsKey(session.Identity) == false)
                        {
                            _clientSessions.Add(session.Identity, session);
                        }
                        break;
                }
            }
        }

        public void RemoveSession(string identity)
        {
            lock (_syncSessions)
            {
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    _clientSessions.Remove(identity);
                }
            }
        }

        public ConnectedPeers GetPeers(string identity)
        {
            lock (_syncSessions)
            {
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    return _clientSessions[identity].Peers;
                }
                return new ConnectedPeers() 
                {
                    Audio = false, Remoting = false, Video = false
                };
            }
        }

        public void UpdateSession(string identity, ConnectedPeers peers, GenericEnums.SessionState sessionState)
        {
            lock (_syncSessions)
            {
                // todo: add session type param if needed
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    Session session = _clientSessions[identity];
                    session.Peers = peers;
                    session.SessionState = sessionState;
                }
            }
        }

        public TransferUptading GetTransferActivity(string identity)
        {
            lock (_syncSessions)
            {
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    return _clientSessions[identity].TransferUpdating;
                }
                return new TransferUptading
                {
                    IsAudioUpdating = false,
                    IsRemotingUpdating = false,
                    IsVideoUpdating = false
                };
            }
        }

        public PendingTransfer GetTransferStatus(string identity)
        {
            lock (_syncSessions)
            {
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    return _clientSessions[identity].PendingTransfer;
                }
                return new PendingTransfer
                    {
                        Audio = false,
                        Remoting = false,
                        Video = false
                    };
            }
        }

        public GenericEnums.SessionState GetSessionState(string identity)
        {
            lock (_syncSessions)
            {
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    return _clientSessions[identity].SessionState;
                }
                return GenericEnums.SessionState.Undefined;
            }
        }

        public IList<string> GetConnectedSessions(GenericEnums.RoomActionType roomType)
        {
            lock (_syncSessions)
            {
                IList<string> sessions = new List<string>();
                if (_clientSessions != null)
                {
                    foreach (Session session in _clientSessions.Values)
                    {
                        switch (roomType)
                        {
                            case GenericEnums.RoomActionType.Audio:
                                if (session.Peers.Audio == true && session.SessionState == GenericEnums.SessionState.Opened)
                                {
                                    sessions.Add(session.Identity);
                                }
                                break;
                            case GenericEnums.RoomActionType.Video:
                                if (session.Peers.Video == true && session.SessionState == GenericEnums.SessionState.Opened)
                                {
                                    sessions.Add(session.Identity);
                                }
                                break;
                            case GenericEnums.RoomActionType.Remoting:
                                if (session.Peers.Remoting == true && session.SessionState == GenericEnums.SessionState.Opened)
                                {
                                    sessions.Add(session.Identity);
                                }
                                break;
                        }
                    }
                }
                return sessions;
            }
        }

        #endregion
    }
}
