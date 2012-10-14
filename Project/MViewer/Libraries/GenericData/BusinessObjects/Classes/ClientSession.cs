using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public class ClientSession : Session
    {
         #region c-tor

        public ClientSession(string identity)
        {
            _sessionType = GenericEnums.SessionType.ClientSession;
            _identity = identity;
            _peers.Audio = false;
            _peers.Video = false;
            _peers.Remoting = false;
        }

        #endregion
    }
}
