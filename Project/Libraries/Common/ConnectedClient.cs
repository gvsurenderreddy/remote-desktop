using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class ConnectedClient
    {
        #region members

        bool _connected;
        string _ip;
        int _id;        
        string _hostname;

        #endregion

        #region c-tor

        public ConnectedClient(string ip, string hostname, int id)
        {
            _ip = ip;
            _hostname = hostname;
            _id = id;
            _connected = true;
        }

        #endregion

        #region proprieties

        public string Ip
        {
            get { return _ip; }
        }

        public int Id
        {
            get { return _id; }
        }

        public string Hostname
        {
            get { return _hostname; }
        }

        public bool Connected
        {
            get { return _connected; }
            set { _connected = value; }
        }

        #endregion
    }
}
