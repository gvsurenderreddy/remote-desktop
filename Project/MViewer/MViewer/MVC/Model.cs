using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using UIControls;
using DataAccessLayer;
using Utils;
using System.Data;
using BusinessLogicLayer;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Abstraction;
using System.Configuration;

namespace MViewer
{
    public class Model : IModel
    {
        #region private members

        IContactsDAL _contactsDAL;
        Identity _identity;
        DataView _dvContacts;

        IClientController _clientController;
        IServerController _serverController;

        ISessionManager _sessionManager;
        // use the below flag for turn off/on security
        readonly bool _useSecurity = bool.Parse(ConfigurationManager.AppSettings["UseSecurity"]);

        #endregion

        #region c-tor

        public Model()
        {
            try
            {
                _contactsDAL = new ContactsRepository();
                _clientController = new ClientController(_useSecurity);
                _sessionManager = new SessionManager();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// method used to initialize the Model module
        /// </summary>
        /// <param name="handlers"></param>
        public void IntializeModel(ControllerEventHandlers handlers)
        {
            try
            {
                _identity = new Identity(SystemConfiguration.Instance.FriendlyName);
                SystemConfiguration.Instance.MyIdentity = _identity.GenerateIdentity(SystemConfiguration.Instance.MyAddress, SystemConfiguration.Instance.Port, SystemConfiguration.Instance.ServicePath);
                _dvContacts = _contactsDAL.LoadContacts(SystemConfiguration.Instance.DataBasePath);
                ContactEndpoint myEndpoint = IdentityResolver.ResolveIdentity(((Identity)Identity).MyIdentity );
                _serverController = new ServerController(myEndpoint, ((Identity)Identity).MyIdentity, handlers, _useSecurity);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to remove WCF client
        /// </summary>
        /// <param name="identity"></param>
        public void RemoveClient(string identity)
        {
            try
            {
                PeerStates peers = SessionManager.GetPeerStatus(identity);
                if (peers.AudioSessionState == GenericEnums.SessionState.Closed
                           && peers.VideoSessionState == GenericEnums.SessionState.Closed
                           && peers.RemotingSessionState == GenericEnums.SessionState.Closed
                           )
                {
                    this.ClientController.RemoveClient(identity);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to send specific file to partner
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="identity"></param>
        public void SendFile(string filePath, string identity)
        {
            try
            {
                // send the file via WCF client
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                ClientController.AddClient(identity);

                // ask for sending permission
                bool hasPermission = ClientController.SendingPermission(Path.GetFileName(filePath), fileStream.Length,
                    identity, _identity.MyIdentity);
                if (hasPermission)
                {
                    byte[] fileContent = new byte[fileStream.Length];
                    fileStream.Read(fileContent, 0, fileContent.Length);
                    ClientController.SendFile(_identity.MyIdentity, fileContent, identity, Path.GetFileName(filePath));
                }
                else
                {
                    MessageBox.Show("Partner refused the transfer", "Transfer denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                RemoveClient(identity);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to notify online contacts of updated friendly name
        /// </summary>
        /// <param name="newFriendlyName"></param>
        public void NotifyContacts(string newFriendlyName)
        {
            try
            {
                // retrieve a list of contact identities and tell them about your new status
                string[] identities = this.GetOnlineContactIdentities();
                foreach (string identity in identities)
                {
                    _clientController.AddClient(identity);
                    _clientController.UpdateFriendlyName(identity, ((Identity)Identity).MyIdentity, newFriendlyName);

                    // remove the client only if it doesn't have any active s
                    PeerStates peers = SessionManager.GetPeerStatus(identity);
                    if (
                        (peers.AudioSessionState == GenericEnums.SessionState.Closed || peers.AudioSessionState == GenericEnums.SessionState.Undefined)
                        && (peers.RemotingSessionState == GenericEnums.SessionState.Closed || peers.RemotingSessionState == GenericEnums.SessionState.Undefined)
                        && (peers.VideoSessionState == GenericEnums.SessionState.Closed || peers.VideoSessionState == GenericEnums.SessionState.Undefined))
                    {
                        RemoveClient(identity);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to notify online contacts of new status
        /// </summary>
        /// <param name="newStatus"></param>
        public void NotifyContacts(GenericEnums.ContactStatus newStatus)
        {
            try
            {
                // retrieve a list of contact identities and tell them about your new status
                string[] identities = this.GetOnlineContactIdentities();
                foreach (string identity in identities)
                {
                    _clientController.AddClient(identity);
                    _clientController.UpdateContactStatus(identity, ((Identity)Identity).MyIdentity, newStatus);
                    RemoveClient(identity);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to retrieve contact details
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public ContactBase GetContact(string identity)
        {
            return _contactsDAL.GetContactByIdentity(identity);
        }

        /// <summary>
        /// method used to retrieve contacts status
        /// </summary>
        /// <param name="pingIdentity"></param>
        public void PingContacts(string pingIdentity)
        {
            try
            {
                if (string.IsNullOrEmpty(pingIdentity))
                {
                    int toPing = _dvContacts.DataViewManager.DataSet.Tables[0].Rows.Count;
                    int pinged = 0;
                    // ping all contacts to get their status
                    foreach (DataRow contact in _dvContacts.DataViewManager.DataSet.Tables[0].Rows)
                    {
                        Thread t = new Thread(delegate()
                        {
                            string identity = contact["Identity"].ToString();
                            bool isOnline = _clientController.IsContactOnline(identity);
                            contact["Status"] = isOnline == true ? GenericEnums.ContactStatus.Online.ToString()
                                : GenericEnums.ContactStatus.Offline.ToString();
                            pinged++;
                        });
                        t.Start();
                    }
                    while (pinged < toPing)
                    {
                        Thread.Sleep(100);
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// method used to execute CRUD specific contact operation
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public ContactBase PerformContactOperation(EventArgs e)
        {
            ContactsEventArgs args = (ContactsEventArgs)e;
            Contact updatedContact = (Contact)args.UpdatedContact;
            Contact contact = null;
            try
            {
                switch (args.Operation)
                {
                    case GenericEnums.ContactsOperation.Status:

                        // check if the contact is still in your list, otherwise send contact removal request to the partner
                        contact = (Contact)_contactsDAL.GetContactByIdentity(updatedContact.Identity);
                        if (contact != null)
                        {
                            if (updatedContact.Status == GenericEnums.ContactStatus.Offline)
                            {
                                UpdateContactStatus(updatedContact.Identity, GenericEnums.ContactStatus.Offline);
                            }
                            else
                            {
                                PingContacts(updatedContact.Identity);
                            }
                        }
                        else
                        {
                            SendRemoveCommand(updatedContact.Identity);
                        }
                        break;
                    case GenericEnums.ContactsOperation.Add:
                        int contactNo = _contactsDAL.AddContact(updatedContact);
                        if (contactNo > -1)
                        {
                            _dvContacts = _contactsDAL.LoadContacts(SystemConfiguration.Instance.DataBasePath);
                            contact = (Contact)_contactsDAL.GetContactByIdentity(updatedContact.Identity);
                            if (updatedContact.ContactNo != -1)
                            {
                                // notify other contact of performed operation (ADD/REMOVE)
                                ClientController.AddClient(contact.Identity);
                                IMViewerService client = ClientController.GetClient(contact.Identity);
                                client.AddContact(_identity.MyIdentity, _identity.FriendlyName);
                            }
                            PingContacts(updatedContact.Identity);
                        }
                        break;
                    case GenericEnums.ContactsOperation.Update:
                        _contactsDAL.UpdateContact(updatedContact);
                        _dvContacts = _contactsDAL.LoadContacts(SystemConfiguration.Instance.DataBasePath);
                        contact = (Contact)_contactsDAL.GetContactByNumber(updatedContact.ContactNo);
                        break;
                    case GenericEnums.ContactsOperation.Remove:
                        contact = (Contact)_contactsDAL.GetContactByIdentity(updatedContact.Identity);
                        if (contact != null)
                        {
                            _contactsDAL.RemoveContact(contact.ContactNo);
                        }
                        _dvContacts = _contactsDAL.LoadContacts(SystemConfiguration.Instance.DataBasePath);
                        if (updatedContact.ContactNo != -1)
                        {
                            SendRemoveCommand(contact.Identity);
                        }
                        break;
                    case GenericEnums.ContactsOperation.Get:
                        contact = (Contact)_contactsDAL.GetContactByNumber(updatedContact.ContactNo);
                        break;
                    case GenericEnums.ContactsOperation.Load:
                        _dvContacts = _contactsDAL.LoadContacts(SystemConfiguration.Instance.DataBasePath);
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return contact;
        }

        #endregion

        #region private methods

        void SendRemoveCommand(string identity)
        {
            ClientController.AddClient(identity);
            IMViewerService client2 = ClientController.GetClient(identity);
            Thread t = new Thread(delegate()
            {
                try
                {
                    bool isOnline = ClientController.IsContactOnline(identity);
                    if (isOnline)
                    {
                        client2.RemoveContact(_identity.MyIdentity);
                    }
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
            });
            t.Start();
        }

        string[] GetOnlineContactIdentities()
        {
            try
            {
                return _dvContacts.DataViewManager.DataSet.Tables[0].AsEnumerable().
                    Where(s => s.Field<string>("Status") == GenericEnums.ContactStatus.Online.ToString()
                    ).Select(s => s.Field<string>("Identity")).ToArray<string>();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return null;
        }

        void UpdateContactStatus(string identity, GenericEnums.ContactStatus newStatus)
        {
            try
            {
                IEnumerable<DataRow> enumerable = _dvContacts.DataViewManager.DataSet.Tables[0].AsEnumerable().
                    Where(s => s.Field<string>("Identity").Equals(identity));
                if (enumerable.Count() > 0)
                {
                    DataRow contact = enumerable.ElementAt(0);
                    if (contact != null)
                    {
                        contact["Status"] = newStatus.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
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

        public IdentityBase Identity
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
