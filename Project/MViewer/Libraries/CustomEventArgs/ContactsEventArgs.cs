using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utils;
using Abstraction;

namespace GenericObjects
{
    public class ContactsEventArgs : EventArgs
    {
        public DataView ContactsDV
        {
            get;
            set;
        }

        public ContactBase UpdatedContact
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
