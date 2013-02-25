using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using UIControls;
using DataAccessLayer;
using Utils;
using System.Data;
using BusinessLogicLayer;

namespace MViewer
{
    public class Model : IModel
    {
        #region private members

        Identity _identity;
        DataView _dvContacts;

        IClientController _clientController;
        IServerController _serverController;

        ISessionManager _sessionManager;
        
        #endregion

        #region c-tor

        public Model(ControllerEventHandlers handlers)
        {
            _identity = new Identity(SystemConfiguration.FriendlyName);
            SystemConfiguration.MyIdentity = _identity.GenerateIdentity(SystemConfiguration.MyAddress, SystemConfiguration.Port, SystemConfiguration.ServicePath);
            _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
            _clientController = new ClientController();

            ContactEndpoint myEndpoint = IdentityResolver.ResolveIdentity(Identity.MyIdentity);
            _serverController = new ServerController(myEndpoint, Identity.MyIdentity, handlers);
        
            _sessionManager = new SessionManager();
        }

        #endregion

        #region public methods

        public void NotifyContacts(string newFriendlyName)
        {
            // retrieve a list of contact identities and tell them about your new status
            string[] identities = this.GetOnlineContactIdentities();
            foreach (string identity in identities)
            {
                _clientController.AddClient(identity);
                _clientController.UpdateFriendlyName(identity, Identity.MyIdentity, newFriendlyName);

                //todo: remove the client only if it doesn't have any active chats
                _clientController.RemoveClient(identity);
            }
        }

        public void NotifyContacts(GenericEnums.ContactStatus newStatus)
        {
            // retrieve a list of contact identities and tell them about your new status
            string[] identities = this.GetOnlineContactIdentities();
            foreach (string identity in identities)
            {
                _clientController.AddClient(identity);
                _clientController.UpdateContactStatus(identity, Identity.MyIdentity, newStatus);
                _clientController.RemoveClient(identity);
            }
        }

        public Contact GetContact(string identity)
        {
            return ContactsRepository.GetContact(identity);
        }

        public void PingContacts(string pingIdentity)
        {
            if (string.IsNullOrEmpty(pingIdentity))
            {
                // ping all contacts to get their status
                foreach (DataRow contact in _dvContacts.DataViewManager.DataSet.Tables[0].Rows)
                {
                    string identity = contact["Identity"].ToString();
                    try
                    {
                        bool isOnline = _clientController.IsContactOnline(identity);
                        contact["Status"] = isOnline == true ? GenericEnums.ContactStatus.Online : GenericEnums.ContactStatus.Offline;
                    }
                    catch (Exception)
                    {
                        contact["Status"] = GenericEnums.ContactStatus.Offline;
                    }
                }
            }
            else
            {
                // ping only the specified contact
                bool isOnline = _clientController.IsContactOnline(pingIdentity);
                UpdateContactStatus(pingIdentity, isOnline == true ? GenericEnums.ContactStatus.Online
                    : GenericEnums.ContactStatus.Offline);
            }
        }

        public Contact PerformContactOperation(ContactsEventArgs e)
        {
            Contact contact = null;
            switch (e.Operation)
            {
                case GenericEnums.ContactsOperation.Status:
                    PingContacts(e.UpdatedContact.Identity);
                    break;
                case GenericEnums.ContactsOperation.Add:
                    int contactNo = ContactsRepository.AddContact(e.UpdatedContact);
                    _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
                    contact = ContactsRepository.GetContact(e.UpdatedContact.Identity);
                    if(e.UpdatedContact.ContactNo != -1)
                    {
                        // notify other contact of performed operation (ADD/REMOVE)
                        ClientController.AddClient(contact.Identity);
                        MViewerClient client = ClientController.GetClient(contact.Identity);
                        client.AddContact(_identity.MyIdentity, _identity.FriendlyName);
                    }
                    PingContacts(null);
                    break;
                case GenericEnums.ContactsOperation.Update:
                    ContactsRepository.UpdateContact(e.UpdatedContact);
                    _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
                    contact = ContactsRepository.GetContact(e.UpdatedContact.ContactNo);
                    break;
                case GenericEnums.ContactsOperation.Remove:
                    contact = ContactsRepository.GetContact(e.UpdatedContact.Identity);
                    ContactsRepository.RemoveContact(contact.ContactNo);
                    _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
                    
                    if (e.UpdatedContact.ContactNo != -1)
                    {
                        ClientController.AddClient(contact.Identity);
                        MViewerClient client2 = ClientController.GetClient(contact.Identity);
                        client2.RemoveContact(_identity.MyIdentity);
                    }
                    break;
                case GenericEnums.ContactsOperation.Get:
                    contact = ContactsRepository.GetContact(e.UpdatedContact.ContactNo);
                    break;
                case GenericEnums.ContactsOperation.Load:
                    _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
                    break;
            }
            return contact;
        }

        #endregion

        #region private methods

        string[] GetOnlineContactIdentities()
        {
            return _dvContacts.DataViewManager.DataSet.Tables[0].AsEnumerable().
                Where(s => s.Field<string>("Status") == GenericEnums.ContactStatus.Online.ToString()
                ).Select(s => s.Field<string>("Identity")).ToArray<string>();
        }

        void UpdateContactStatus(string identity, GenericEnums.ContactStatus newStatus)
        {
            try
            {
                DataRow contact = _dvContacts.DataViewManager.DataSet.Tables[0].AsEnumerable().
                    Where(s => s.Field<string>("Identity") == identity).First();
                if (contact != null)
                {
                    contact["Status"] = newStatus.ToString();
                }
            }
            catch
            {

            }
        }

        #endregion

        #region proprieties

        public ISessionManager SessionManager
        {
            get { return _sessionManager; }
        }

        public string FriendlyName
        {
            get { return _identity.FriendlyName; }
        }

        public Identity Identity
        {
            get { return _identity; }
        }

        public DataView Contacts
        {
            get { return _dvContacts; }
        }

        public IClientController ClientController
        {
            get { return _clientController; }
        }

        public IServerController ServerController
        {
            get { return _serverController; }
        }

        #endregion
    }
}
