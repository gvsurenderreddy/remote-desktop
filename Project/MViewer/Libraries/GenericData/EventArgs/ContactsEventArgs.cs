using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utils;

namespace GenericDataLayer
{
    public class ContactsEventArgs : EventArgs
    {
        public DataView ContactsDV
        {
            get;
            set;
        }

        public Contact UpdatedContact
        {
            get;
            set;
        }

        public GenericEnums.ContactsOperation Operation
        {
            get;
            set;
        }
    }
}
