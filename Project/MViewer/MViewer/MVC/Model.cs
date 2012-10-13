using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using UIControls;
using DataAccessLayer;
using Utils;
using System.Data;

namespace MViewer
{
    public class Model : IModel
    {
        #region private members

        Identity _identity;
        DataView _dvContacts;

        #endregion

        #region c-tor

        public Model()
        {
            //IdentityUpdatedEvent += new EventHandlers.IdentityEventHandler(IdentityUpdated);
            _identity = new Identity(SystemConfiguration.FriendlyName);
            SystemConfiguration.MyIdentity = _identity.GenerateIdentity(SystemConfiguration.MyAddress, SystemConfiguration.Port, SystemConfiguration.ServicePath);
            _dvContacts = ContactsRepository.LoadContacts(SystemConfiguration.DataBasePath);
        }

        #endregion

        #region public methods

        public void ContactsUpdated()
        {
            // todo: implement ContactsUpdated
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

        #endregion
    }
}
