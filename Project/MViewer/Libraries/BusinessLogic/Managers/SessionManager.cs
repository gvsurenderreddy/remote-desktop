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
            if (_clientSessions != null && _clientSessions.ContainsKey(identity))
            {
                Session session = _clientSessions[identity];
                session.Peers = peers;
                session.SessionState = sessionState;
            }
        }

        #endregion
    }
}
