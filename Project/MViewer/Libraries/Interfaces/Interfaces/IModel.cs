using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utils;
using Abstraction;

namespace GenericObjects
{
    public interface IModel
    {
        ContactBase PerformContactOperation(EventArgs e);
        void PingContacts(string identity);
        ContactBase GetContact(string identity);
        void NotifyContacts(GenericEnums.ContactStatus newStatus);
        void NotifyContacts(string newFriendlyName);
        void SendFile(string filePath, string identity);
        void RemoveClient(string identity);
        void IntializeModel(ControllerEventHandlers handlers);

        ISessionManager SessionManager
        {
            get;
        }

        IdentityBase Identity
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
