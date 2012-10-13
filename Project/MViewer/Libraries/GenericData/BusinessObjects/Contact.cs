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
        //Utils.GenericEnums.ContactStatus _status;      

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

        public int ContactNo
        {
            get { return _contactNo; }
        }

        //public string Status
        //{
        //    get { return _status.ToString(); }
        //}

        //public void SetStatus(GenericEnums.ContactStatus status)
        //{
        //    _status = status;
        //}

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
