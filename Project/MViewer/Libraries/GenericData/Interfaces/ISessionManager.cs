using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public interface ISessionManager
    {
        void AddSession(string identity, Session session);
        void RemoveSession(string identity);
        void UpdateSession(string identity, ConnectedPeers peers);
    }
}
