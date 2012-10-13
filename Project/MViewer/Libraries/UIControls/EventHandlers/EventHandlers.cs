using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;

namespace UIControls
{
    public static class EventHandlers
    {
        public delegate void IdentityEventHandler(object o, IdentityEventArgs e);
        public delegate void ContactsEventHandler(object o, ContactsEventArgs e);
        public delegate void ActionsEventHandler(object o, FrontEndActionsEventArgs e);

    }
}
