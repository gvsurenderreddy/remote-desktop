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
        IDictionary<string, Session> _serverSessions;

        #endregion

        #region public methods

        public void AddSession(Session session)
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
                case GenericEnums.SessionType.ServerSession:
                    if (_serverSessions == null)
                    {
                        _serverSessions = new Dictionary<string, Session>();
                    }
                    if (_serverSessions.ContainsKey(session.Identity) == false)
                    {
                        _serverSessions.Add(session.Identity, session);
                    }
                    break;
            }
        }

        public void RemoveSession(string identity)
        {
            if (_clientSessions != null && _clientSessions.ContainsKey(identity))
            {
                _clientSessions.Remove(identity);
            }

        }

        public void UpdateSession(string identity, ConnectedPeers peers, GenericEnums.SessionState sessionState)
        {
            // todo: add session type param if needed
            if (_clientSessions != null && _clientSessions.ContainsKey(identity))
            {
                Session session = _clientSessions[identity];
                session.Peers = peers;
                session.SessionState = sessionState;
            }
        }

        public IList<string> GetConnectedSessions(GenericEnums.SessionType sessionType, GenericEnums.RoomActionType actionType)
        {
            IList<string> sessions = new List<string>();
            switch(sessionType)
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
                    if (_serverSessions != null)
                    {
                        foreach (Session session in _serverSessions.Values)
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
            }
          
            return sessions;
        }

        #endregion
    }
}
