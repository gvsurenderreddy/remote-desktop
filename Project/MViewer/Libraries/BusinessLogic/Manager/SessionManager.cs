using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using Utils;

namespace BusinessLogicLayer
{
    public class SessionManager : ISessionManager
    {
        #region private members

        IDictionary<string, ClientSession> _clientSessions;
        readonly object _syncSessions = new object();

        #endregion

        #region public methods

        public bool RemotingRoomsLeft()
        {
            bool left = false;
            lock (_syncSessions)
            {
                if (_clientSessions != null)
                {
                    foreach (ClientSession session in _clientSessions.Values)
                    {
                        if (session.RemotingSessionState != GenericEnums.SessionState.Closed)
                        {
                            left = true;
                            break;
                        }
                    }
                }
            }
            return left;
        }

        public void AddSession(Session session, GenericEnums.RoomType scope)
        {
            try
            {
                lock (_syncSessions)
                {
                    if (_clientSessions == null)
                    {
                        _clientSessions = new Dictionary<string, ClientSession>();
                    }
                    if (_clientSessions.ContainsKey(((ClientSession)session).Identity) == false)
                    {
                        _clientSessions.Add(((ClientSession)session).Identity, (ClientSession)session);
                    }
                    else
                    {
                        switch (scope)
                        {
                            case GenericEnums.RoomType.Audio:
                                _clientSessions[((ClientSession)session).Identity].AudioSessionState = ((ClientSession)session).Peers.AudioSessionState;
                                break;
                            case GenericEnums.RoomType.Remoting:
                                _clientSessions[((ClientSession)session).Identity].RemotingSessionState = ((ClientSession)session).Peers.RemotingSessionState;
                                break;
                            case GenericEnums.RoomType.Video:
                                _clientSessions[((ClientSession)session).Identity].VideoSessionState = ((ClientSession)session).Peers.VideoSessionState;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void RemoveSession(string identity)
        {
            try
            {
                lock (_syncSessions)
                {
                    if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                    {
                        if (
                            (_clientSessions[identity].AudioSessionState == GenericEnums.SessionState.Closed
                            || _clientSessions[identity].AudioSessionState == GenericEnums.SessionState.Undefined)

                            && (_clientSessions[identity].VideoSessionState == GenericEnums.SessionState.Closed
                            || _clientSessions[identity].VideoSessionState == GenericEnums.SessionState.Undefined)

                            && (_clientSessions[identity].RemotingSessionState == GenericEnums.SessionState.Closed
                            || _clientSessions[identity].RemotingSessionState == GenericEnums.SessionState.Undefined)
                            )
                        {
                            _clientSessions.Remove(identity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public PeerStates GetPeerStatus(string identity)
        {
            lock (_syncSessions)
            {
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    return _clientSessions[identity].Peers;
                }
                return new PeerStates() 
                {
                    AudioSessionState = GenericEnums.SessionState.Undefined,
                    RemotingSessionState = GenericEnums.SessionState.Undefined,
                    VideoSessionState = GenericEnums.SessionState.Undefined
                };
            }
        }

        public TransferStatusUptading GetTransferActivity(string identity)
        {
            lock (_syncSessions)
            {
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    return _clientSessions[identity].TransferUpdating;
                }
                return new TransferStatusUptading
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

        public GenericEnums.SessionState GetSessionState(string identity, GenericEnums.RoomType roomType)
        {
            GenericEnums.SessionState returnState = GenericEnums.SessionState.Undefined;
            lock (_syncSessions)
            {
                if (_clientSessions != null && _clientSessions.ContainsKey(identity))
                {
                    switch (roomType)
                    {
                        case GenericEnums.RoomType.Video:
                            returnState = _clientSessions[identity].VideoSessionState;
                            break;
                        case GenericEnums.RoomType.Audio:
                            returnState = _clientSessions[identity].AudioSessionState;
                            break;
                        case GenericEnums.RoomType.Remoting:
                            returnState = _clientSessions[identity].RemotingSessionState;
                            break;
                    }
                  
                }
            }
            return returnState;
        }

        public IList<string> GetConnectedSessions(GenericEnums.RoomType roomType)
        {
            lock (_syncSessions)
            {
                IList<string> sessions = new List<string>();
                if (_clientSessions != null)
                {
                    foreach (ClientSession session in _clientSessions.Values)
                    {
                        switch (roomType)
                        {
                            case GenericEnums.RoomType.Audio:
                                if (session.AudioSessionState == GenericEnums.SessionState.Opened
                                    || session.AudioSessionState == GenericEnums.SessionState.Pending
                                    || session.AudioSessionState == GenericEnums.SessionState.Paused)
                                {
                                    sessions.Add(session.Identity);
                                }
                                break;
                            case GenericEnums.RoomType.Video:
                                if (session.VideoSessionState == GenericEnums.SessionState.Opened
                                    || session.VideoSessionState == GenericEnums.SessionState.Pending
                                    || session.VideoSessionState == GenericEnums.SessionState.Paused)
                                {
                                    sessions.Add(session.Identity);
                                }
                                break;
                            case GenericEnums.RoomType.Remoting:
                                if (session.RemotingSessionState == GenericEnums.SessionState.Opened
                                    || session.RemotingSessionState == GenericEnums.SessionState.Pending
                                    || session.RemotingSessionState == GenericEnums.SessionState.Paused)
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
