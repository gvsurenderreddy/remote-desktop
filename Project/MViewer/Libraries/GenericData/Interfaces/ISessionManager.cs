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
        PeerStatus GetPeerStatus(string identity);
        void UpdateSession(string identity, PeerStatus peers, GenericEnums.SessionState sessionState);
        IList<string> GetConnectedSessions(GenericEnums.RoomActionType actionType);
        GenericEnums.SessionState GetSessionState(string identity);
        PendingTransfer GetTransferStatus(string identity);
        TransferUptading GetTransferActivity(string identity);
    }
}
