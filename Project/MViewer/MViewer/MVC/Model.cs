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

        IPresenterManager _presenterManager;
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
            _presenterManager = new PresenterManager();
            _sessionManager = new SessionManager();
        }

        #endregion

        #region public methods

        public Contact GetContact(string identity)
        {
            return ContactsRepository.GetContact(identity);
        }

        public void PingContacts()
        {
            // todo: ping all contacts to get their status
            foreach (DataRow contact in _dvContacts.DataViewManager.DataSet.Tables[0].Rows)
            {
                string identity = contact["Identity"].ToString();
                try
                {
                    bool isOnline = _clientController.IsContactOnline(identity);
                    contact["Status"] = isOnline == true ? GenericEnums.ContactStatus.Online : GenericEnums.ContactStatus.Offline;
                }
                catch (Exception ex)
                {
                    contact["Status"] = GenericEnums.ContactStatus.Offline;
                }
            }
        }

        public Contact PerformContactOperation(ContactsEventArgs e)
        {
            Contact contact = null;

            switch (e.Operation)
            {
                case GenericEnums.ContactsOperation.Add:
                    int contactNo = ContactsRepository.AddContact(e.UpdatedContact);
                    _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
                    contact = ContactsRepository.GetContact(contactNo);
                    PingContacts();
                    break;
                case GenericEnums.ContactsOperation.Update:
                    ContactsRepository.UpdateContact(e.UpdatedContact);
                    _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
                    contact = ContactsRepository.GetContact(e.UpdatedContact.ContactNo);
                    break;
                case GenericEnums.ContactsOperation.Remove:
                    ContactsRepository.RemoveContact(e.UpdatedContact.ContactNo);
                    _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
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

        #region proprieties

        public ISessionManager SessionManager
        {
            get { return _sessionManager; }
        }

        public IPresenterManager PresenterManager
        {
            get { return _presenterManager; }
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
