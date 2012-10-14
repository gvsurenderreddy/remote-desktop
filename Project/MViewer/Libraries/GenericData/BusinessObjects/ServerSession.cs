using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public class ServerSession : Session
    {
        #region c-tor

        public ServerSession(string identity, GenericEnums.SessionType sessionType)
        {
            _sessionType = sessionType;
            _identity = identity;
            _peers.Audio = false;
            _peers.Video = false;
            _peers.Remoting = false;
        }

        #endregion

        #region public methods



        #endregion
    }
}
