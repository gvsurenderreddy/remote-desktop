using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;

namespace BusinessLogicLayer
{
    public class SessionManager : ISessionManager
    {
        #region private members

        IDictionary<string, Session> _sessions;

        #endregion

        #region public methods

        public void AddSession(string identity, Session session)
        {
            if (_sessions == null)
            {
                _sessions = new Dictionary<string, Session>();
            }
            if (_sessions.ContainsKey(identity) == false)
            {
                _sessions.Add(identity, session);
            }
        }

        public void RemoveSession(string identity)
        {
            if (_sessions != null && _sessions.ContainsKey(identity))
            {
                _sessions.Remove(identity);
            }

        }

        public void UpdateSession(string identity, ConnectedPeers peers)
        {
            if (_sessions != null && _sessions.ContainsKey(identity))
            {
                Session session = _sessions[identity];
                session.Peers = peers;
            }
        }

        #endregion
    }
}
