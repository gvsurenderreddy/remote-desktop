﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utils;

namespace GenericDataLayer
{
    public interface IModel
    {
        Contact PerformContactOperation(ContactsEventArgs e);
        void PingContacts();
        Contact GetContact(string identity);

        IPresenterManager PresenterManager
        {
            get;
        }

        ISessionManager SessionManager
        {
            get;
        }

        Identity Identity
        {
            get;
        }

        string FriendlyName
        {
            get;
        }

        DataView Contacts
        {
            get;
        }

        IClientController ClientController
        {
            get;
        }

        IServerController ServerController
        {
            get;
        }
    }
}
