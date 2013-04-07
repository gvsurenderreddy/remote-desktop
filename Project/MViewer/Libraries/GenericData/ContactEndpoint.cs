using Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class ContactEndpoint : ContactEndpointBase
    {
        #region private members

        string _address;      
        int _port;
        string _path;

        #endregion

        #region c-tor

        public ContactEndpoint(string address, int port, string path)
        {
            _address = address;
            _port = port;
            _path = path;
        }

        #endregion

        #region proprieties

        public override string Address
        {
            get { return _address; }
        }

        public override int Port
        {
            get { return _port; }
        }

        public override string Path
        {
            get { return _path; }
        }

        #endregion
    }
}
