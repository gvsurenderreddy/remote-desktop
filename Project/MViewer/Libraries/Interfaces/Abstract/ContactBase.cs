using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Abstraction
{
    public abstract class ContactBase
    {
        #region proprieties

        public int ContactNo
        {
            get;
            set;
        }

        public string FriendlyName
        {
            get;
            set;
        }

        public string Identity
        {
            get;
            set;
        }

        public GenericEnums.ContactStatus Status
        {
            get;
            set;
        }

        #endregion
    }
}
