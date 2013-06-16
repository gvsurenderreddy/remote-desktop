using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using System.Data;
using System.IO;
using Utils;
using Abstraction;

namespace DataAccessLayer
{
    public class ContactsRepository : IContactsDAL
    {
        #region private members

        string _xmlFilePath;
        DataSet _contactsDataSet;
        //DataView _contactsDataView;

        #endregion

        #region c-tor

        public ContactsRepository()
        {
            _contactsDataSet = new DataSet();
        }

        #endregion

        #region public methods

        public int GetContactsCount()
        {
            int contactsNo = 0;
            if (_contactsDataSet.Tables.Count > 0)
            {
                contactsNo = _contactsDataSet.Tables[0].Rows.Count;
            }
            return contactsNo;
        }

        public string[] GetContactIdentities()
        {
            return _contactsDataSet.Tables[0].AsEnumerable().Select(s => s.Field<string>("Identity")).ToArray<string>();
        }

        public DataSet LoadContacts(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _contactsDataSet = new DataSet();
            //_contactsDataView = new DataView();

            _contactsDataSet.Clear();
            if (File.Exists(_xmlFilePath))
            {
                _contactsDataSet.ReadXml(_xmlFilePath, XmlReadMode.ReadSchema);
                //_contactsDataView = _contactsDataSet.Tables[0].DefaultView;
            }
            else
            {
                DataTable dtContacts = new DataTable("Contacts");
                dtContacts.Columns.Add(new DataColumn("ContactNo", typeof(string)));
                dtContacts.Columns.Add(new DataColumn("Identity", typeof(string)));
                dtContacts.Columns.Add(new DataColumn("FriendlyName", typeof(string)));
                dtContacts.Columns.Add(new DataColumn("Status", typeof(string)));
                
                _contactsDataSet.Tables.Add(dtContacts);
                //_contactsDataView = _contactsDataSet.Tables[0].DefaultView;
            }
            return _contactsDataSet;
        }

        public int AddContact(ContactBase contact)
        {
            ContactBase existingContact = GetContactByIdentity(contact.Identity);
            if(existingContact != null)
            {
                return -1;
            }
            else
            {
                int maxContactNo = GetMaxContactNumber();
                DataRow dr = _contactsDataSet.Tables[0].NewRow();
                dr["ContactNo"] = ++maxContactNo;
                dr["FriendlyName"] = contact.FriendlyName;
                dr["Identity"] = contact.Identity;
                _contactsDataSet.Tables[0].Rows.Add(dr);
                SaveContacts();
                LoadContacts(_xmlFilePath);
                return int.Parse(dr["ContactNo"].ToString());
            }
        }

        public void RemoveContact(int contactNo)
        {
            try
            {
                var contacts = from product in _contactsDataSet.Tables[0].AsEnumerable()
                               where product["ContactNo"].ToString().Equals(contactNo.ToString())
                               select product;
                DataRow contact = contacts.First();
                _contactsDataSet.Tables[0].Rows.Remove(contact);
                SaveContacts();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void UpdateContact(ContactBase contact)
        {
            try
            {
                var contacts = from product in _contactsDataSet.Tables[0].AsEnumerable()
                               where product["ContactNo"].ToString().Equals(contact.ContactNo.ToString())
                               select product;
                DataRow toUpdate = contacts.First();
                if (toUpdate != null)
                {
                    toUpdate.SetField("FriendlyName", contact.FriendlyName);
                    this.SaveContacts();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public ContactBase GetContactByNumber(int contactNo)
        {
            Contact toReturn = null;  
            var contacts = from product in _contactsDataSet.Tables[0].AsEnumerable()
                               where product["ContactNo"].ToString().Equals(contactNo.ToString())
                               select product;
            DataRow contact = contacts.First();
            if (contact != null)
            {
                toReturn = new Contact(
                    int.Parse(contact["ContactNo"].ToString()),
                    contact["FriendlyName"].ToString(),
                    contact["Identity"].ToString());
            }
            return toReturn;
        }

        public ContactBase GetContactByIdentity(string identity)
        {
            DataTable contacts = _contactsDataSet.Tables["Contacts"];
            IEnumerable<DataRow> query =
                from product in contacts.AsEnumerable()
                where product["Identity"].ToString().ToLower().Equals(identity.ToLower())
                select product;
            Contact contact = null;
            if (query != null && query.Count() > 0)
            {
                contact = new Contact(int.Parse(query.ElementAt(0)["ContactNo"].ToString()), query.ElementAt(0)["FriendlyName"].ToString(), query.ElementAt(0)["Identity"].ToString());
            }
            return contact;
        }

        public void SaveContacts()
        {
            try
            {
                _contactsDataSet.WriteXml(_xmlFilePath, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        int GetMaxContactNumber()
        {
            int maxContactNo = 0;
            var dataTable = _contactsDataSet.Tables[0].AsEnumerable();
            var contactNumbers = (from contact in dataTable
                                  select contact["contactNo"]);
            if (contactNumbers != null && contactNumbers.Count() > 0)
            {
                maxContactNo = int.Parse(contactNumbers.Max().ToString());
            }
            else
            {
                maxContactNo = 0;
            }
            return maxContactNo;
        }

        #endregion
    }
}
