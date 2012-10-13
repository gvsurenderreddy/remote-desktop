using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericDataLayer
{
    public class Contact
    {
        #region private members

        int _contactNo;
        string _friendlyName;
        string _identity;
        GenericEnums.ContactStatus _status;
        ConnectedPeers _connectedPeers = new ConnectedPeers();

        #endregion

        #region c-tor

        public Contact(int contactNo, string friendlyName, string identity)
        {
            _contactNo = contactNo;
            _friendlyName = friendlyName;
            _identity = identity;
            //_status = Utils.GenericEnums.ContactStatus.Offline;
        }

        #endregion

        #region public methods


        #endregion

        #region proprieties

        public ConnectedPeers ConnectedPeers
        {
            get { return _connectedPeers; }
            set { _connectedPeers = value; }
        }

        public int ContactNo
        {
            get { return _contactNo; }
        }

        public GenericEnums.ContactStatus Status
        {
            get { return _status; }
            set 
            {
                _status = value; 
            }
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
