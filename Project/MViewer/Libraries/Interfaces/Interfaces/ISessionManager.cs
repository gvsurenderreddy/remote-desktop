using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericObjects
{
    public interface ISessionManager
    {
        void AddSession(Session session, GenericEnums.RoomType scope);
        void RemoveSession(string identity);
        PeerStates GetPeerStatus(string identity);

        bool RemotingRoomsLeft();
        GenericEnums.SessionState GetSessionState(string identity, GenericEnums.RoomType roomType);
        PendingTransfer GetTransferStatus(string identity);
        ConferenceStatus GetConferenceStatus(string identity);
        IList<string> GetConnectedSessions(GenericEnums.RoomType actionType);
    }
}
