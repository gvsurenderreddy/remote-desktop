using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public interface ISessionManager
    {
        void AddSession(Session session);
        void RemoveSession(string identity);
        void UpdateSession(string identity, ConnectedPeers peers, GenericEnums.SessionState sessionState);
        IList<string> GetConnectedSessions(GenericEnums.SessionType sessionType, GenericEnums.RoomActionType actionType);
    }
}
