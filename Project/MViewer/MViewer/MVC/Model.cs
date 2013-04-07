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

        public Model()
        {
            try
            {
                _clientController = new ClientController();
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
                _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.Instance.DataBasePath);
                ContactEndpoint myEndpoint = IdentityResolver.ResolveIdentity(Identity.MyIdentity);
                _serverController = new ServerController(myEndpoint, Identity.MyIdentity, handlers);
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
                    ClientController.RemoveClient(identity);
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
                    ClientController.SendFile(fileContent, identity, Path.GetFileName(filePath));
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
                    _clientController.UpdateFriendlyName(identity, Identity.MyIdentity, newFriendlyName);

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
                    _clientController.UpdateContactStatus(identity, Identity.MyIdentity, newStatus);
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
            return ContactsRepository.GetContactByIdentity(identity);
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
                    // ping all contacts to get their status
                    foreach (DataRow contact in _dvContacts.DataViewManager.DataSet.Tables[0].Rows)
                    {
                        string identity = contact["Identity"].ToString();
                        bool isOnline = _clientController.IsContactOnline(identity);
                        contact["Status"] = isOnline == true ? GenericEnums.ContactStatus.Online.ToString()
                            : GenericEnums.ContactStatus.Offline.ToString();
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
            Contact contact = null;
            try
            {
                switch (args.Operation)
                {
                    case GenericEnums.ContactsOperation.Status:
                        PingContacts(args.UpdatedContact.Identity);
                        break;
                    case GenericEnums.ContactsOperation.Add:
                        int contactNo = ContactsRepository.AddContact(args.UpdatedContact);
                        _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.Instance.DataBasePath);
                        contact = ContactsRepository.GetContactByIdentity(args.UpdatedContact.Identity);
                        if (args.UpdatedContact.ContactNo != -1)
                        {
                            // notify other contact of performed operation (ADD/REMOVE)
                            ClientController.AddClient(contact.Identity);
                            IMviewerChannel client = ClientController.GetClient(contact.Identity);
                            client.AddContact(_identity.MyIdentity, _identity.FriendlyName);
                        }
                        PingContacts(null);
                        break;
                    case GenericEnums.ContactsOperation.Update:
                        ContactsRepository.UpdateContact(args.UpdatedContact);
                        _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.Instance.DataBasePath);
                        contact = ContactsRepository.GetContactByNumber(args.UpdatedContact.ContactNo);
                        break;
                    case GenericEnums.ContactsOperation.Remove:
                        contact = ContactsRepository.GetContactByIdentity(args.UpdatedContact.Identity);
                        ContactsRepository.RemoveContact(contact.ContactNo);
                        _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.Instance.DataBasePath);

                        if (args.UpdatedContact.ContactNo != -1)
                        {
                            ClientController.AddClient(contact.Identity);
                            IMviewerChannel client2 = ClientController.GetClient(contact.Identity);
                            Thread t = new Thread(delegate()
                            {
                                try
                                {
                                    bool isOnline = ClientController.IsContactOnline(contact.Identity);
                                    if (isOnline)
                                    {
                                        client2.RemoveContact(_identity.MyIdentity);
                                    }
                                    else
                                    {
                                        //todo: create a message queue so that the partner will be notified of removal
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Tools.Instance.Logger.LogError(ex.ToString());
                                }
                            });
                            t.Start();
                        }
                        break;
                    case GenericEnums.ContactsOperation.Get:
                        contact = ContactsRepository.GetContactByNumber(args.UpdatedContact.ContactNo);
                        break;
                    case GenericEnums.ContactsOperation.Load:
                        _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.Instance.DataBasePath);
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
                DataRow contact = _dvContacts.DataViewManager.DataSet.Tables[0].AsEnumerable().
                    Where(s => s.Field<string>("Identity") == identity).First();
                if (contact != null)
                {
                    contact["Status"] = newStatus.ToString();
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
