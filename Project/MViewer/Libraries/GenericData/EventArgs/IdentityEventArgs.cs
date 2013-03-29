using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public class IdentityEventArgs : EventArgs
    {
        public string FriendlyName
        {
            get;
            set;
        }

        public string MyIdentity
        {
            get;
            set;
        }

        //public string IP
        //{
        //    get;
        //    set;
        //}

        //public int Port
        //{
        //    get;
        //    set;
        //}
    }
}
