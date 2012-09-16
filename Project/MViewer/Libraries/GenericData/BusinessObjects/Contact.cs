using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericData
{
    public class Contact
    {
        #region private members

        string _friendlyName;
        string _identity;
        
        #endregion

        #region c-tor

        public Contact(string friendlyName, string identity)
        {
            _friendlyName = friendlyName;
            _identity = identity;
        }

        #endregion

        #region proprieties

        public string FriendlyName
        {
            get { return _friendlyName; }
            set { _friendlyName = value; }
        }

        public string Identity
        {
            get { return _identity; }
        }

        #endregion
    }
}
