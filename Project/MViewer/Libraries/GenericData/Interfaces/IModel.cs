using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utils;

namespace GenericObjects
{
    public interface IModel
    {
        Contact PerformContactOperation(ContactsEventArgs e);
        void PingContacts(string identity);
        Contact GetContact(string identity);
        void NotifyContacts(GenericEnums.ContactStatus newStatus);
        void NotifyContacts(string newFriendlyName);
        void SendFile(string filePath, string identity);
        void RemoveClient(string identity);
        void IntializeModel(ControllerEventHandlers handlers);

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
