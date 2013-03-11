using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDataLayer
{
    public class Delegates
    {
        public delegate void CommandDelegate(object sender, RoomActionEventArgs args);

        public delegate void IdentityEventHandler(object o, IdentityEventArgs e);
        public delegate void ContactsEventHandler(object o, ContactsEventArgs e);
        public delegate void ActionsEventHandler(object o, RoomActionEventArgs e);
    }
}
