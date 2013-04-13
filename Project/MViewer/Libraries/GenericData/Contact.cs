using Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericObjects
{
    public class Contact : ContactBase
    {
        #region private members

        int _contactNo;
        string _friendlyName;
        string _identity;
        GenericEnums.ContactStatus _status;

        #endregion

        #region c-tor

        public Contact(int contactNo, string friendlyName, string identity)
        {
            _contactNo = contactNo;
            _friendlyName = friendlyName;
            _identity = identity;
            _status = GenericEnums.ContactStatus.Offline;
        }

        public Contact(int contactNo, string identity, GenericEnums.ContactStatus newStatus)
        {
            _contactNo = contactNo;
            _identity = identity;
            _status = newStatus;
            _status = GenericEnums.ContactStatus.Offline;
        }

        #endregion

        #region proprieties

        public int ContactNo
        {
            get { return _contactNo; }
        }

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
