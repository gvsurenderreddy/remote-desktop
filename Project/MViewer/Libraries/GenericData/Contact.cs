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
        #region c-tor

        public Contact(int contactNo, string friendlyName, string identity)
        {
            ContactNo = contactNo;
            FriendlyName = friendlyName;
            Identity = identity;
            Status = GenericEnums.ContactStatus.Offline;
        }

        public Contact(int contactNo, string identity, GenericEnums.ContactStatus newStatus)
        {
            ContactNo = contactNo;
            Identity = identity;
            Status = newStatus;
        }

        #endregion


    }
}
