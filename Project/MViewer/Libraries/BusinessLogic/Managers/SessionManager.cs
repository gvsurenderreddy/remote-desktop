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
        //IDictionary<string, Session> _serverSessions;
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
                    //case GenericEnums.SessionType.ServerSession:
                    //    if (_serverSessions == null)
                    //    {
                    //        _serverSessions = new Dictionary<string, Session>();
                    //    }
                    //    if (_serverSessions.ContainsKey(session.Identity) == false)
                    //    {
                    //        _serverSessions.Add(session.Identity, session);
                    //    }
                    //    break;
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

        public IList<string> GetConnectedSessions(GenericEnums.SessionType sessionType, GenericEnums.RoomActionType actionType)
        {
            lock (_syncSessions)
            {
                IList<string> sessions = new List<string>();
                switch (sessionType)
                {
                    case GenericEnums.SessionType.ClientSession:
                        if (_clientSessions != null)
                        {
                            foreach (Session session in _clientSessions.Values)
                            {
                                switch (actionType)
                                {
                                    case GenericEnums.RoomActionType.Audio:
                                        if (session.Peers.Audio == true)
                                        {
                                            sessions.Add(session.Identity);
                                        }
                                        break;
                                    case GenericEnums.RoomActionType.Video:
                                        if (session.Peers.Video == true)
                                        {
                                            sessions.Add(session.Identity);
                                        }
                                        break;
                                    case GenericEnums.RoomActionType.Remoting:
                                        if (session.Peers.Remoting == true)
                                        {
                                            sessions.Add(session.Identity);
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case GenericEnums.SessionType.ServerSession:
                        //if (_serverSessions != null)
                        //{
                        //    foreach (Session session in _serverSessions.Values)
                        //    {
                        //        switch (actionType)
                        //        {
                        //            case GenericEnums.RoomActionType.Audio:
                        //                if (session.Peers.Audio == true)
                        //                {
                        //                    sessions.Add(session.Identity);
                        //                }
                        //                break;
                        //            case GenericEnums.RoomActionType.Video:
                        //                if (session.Peers.Video == true)
                        //                {
                        //                    sessions.Add(session.Identity);
                        //                }
                        //                break;
                        //            case GenericEnums.RoomActionType.Remoting:
                        //                if (session.Peers.Remoting == true)
                        //                {
                        //                    sessions.Add(session.Identity);
                        //                }
                        //                break;
                        //        }
                        //    }
                        //}
                        break;
                }

                return sessions;
            }
        }

        #endregion
    }
}
