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
        // todo: handle data error event


        #region private members

        string _xmlFilePath;
        DataSet _contactsDataSet;
        DataView _contactsDataView;

        #endregion

        #region c-tor

        public ContactsRepository()
        {
            _contactsDataSet = new DataSet();
            _contactsDataView = new DataView();
        }

        #endregion

        #region public methods

        public string[] GetContactIdentities()
        {
            return _contactsDataSet.Tables[0].AsEnumerable().Select(s => s.Field<string>("Identity")).ToArray<string>();
        }

        public DataView LoadContacts(string xmlFilePath)
        {
            _xmlFilePath = xmlFilePath;
            _contactsDataSet = new DataSet();
            _contactsDataView = new DataView();

            _contactsDataSet.Clear();
            if (File.Exists(_xmlFilePath))
            {
                _contactsDataSet.ReadXml(_xmlFilePath, XmlReadMode.ReadSchema);
                _contactsDataView = _contactsDataSet.Tables[0].DefaultView;
            }
            else
            {
                DataTable dtContacts = new DataTable("Contacts");
                dtContacts.Columns.Add(new DataColumn("ContactNo", typeof(string)));
                dtContacts.Columns.Add(new DataColumn("Identity", typeof(string)));
                dtContacts.Columns.Add(new DataColumn("FriendlyName", typeof(string)));
                dtContacts.Columns.Add(new DataColumn("Status", typeof(string)));
                
                _contactsDataSet.Tables.Add(dtContacts);
                _contactsDataView = _contactsDataSet.Tables[0].DefaultView;
            }
            return _contactsDataView;
        }

        public int AddContact(ContactBase contact)
        {
            DataRow dr = _contactsDataView.Table.NewRow();
            dr["ContactNo"] = dr.Table.Rows.Count;
            dr["FriendlyName"] = contact.FriendlyName;
            dr["Identity"] = contact.Identity;
            _contactsDataView.Table.Rows.Add(dr);
            SaveContacts();
            LoadContacts(_xmlFilePath);
            return int.Parse(dr["ContactNo"].ToString());
        }

        public void RemoveContact(int contactNo)
        {
            try
            {
                _contactsDataView.RowFilter = "ContactNo='" + contactNo + "'";
                _contactsDataView.Sort = "ContactNo";
                _contactsDataView.Delete(0);
                _contactsDataView.RowFilter = "";
                SaveContacts();
                LoadContacts(_xmlFilePath);
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
                DataRow dr = GetContactByNo(contact.ContactNo);
                if (dr == null)
                {
                    Contact toUpdate = (Contact)GetContactByIdentity(contact.Identity);
                    dr = GetContactByNo(toUpdate.ContactNo);
                }
                if (dr != null)
                {
                    dr.SetField("FriendlyName", contact.FriendlyName);
                    SaveContacts();
                    LoadContacts(_xmlFilePath);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public ContactBase GetContactByNumber(int contactNo)
        {
            Contact contact = null;
            if (contactNo >= 0)
            {
                _contactsDataView.RowFilter = "contactno='" + contactNo + "'";
                _contactsDataView.Sort = "identity";
                DataRow dr = null;
                if (_contactsDataView.Count > 0)
                {
                    dr = _contactsDataView[0].Row;
                    if (dr != null)
                    {
                        contact = new Contact(
                         int.Parse(dr["ContactNo"].ToString()),
                         dr["FriendlyName"].ToString(),
                         dr["Identity"].ToString());
                    }
                }
            }
            return contact;
        }

        public ContactBase GetContactByIdentity(string identity)
        {
            DataTable contacts = _contactsDataSet.Tables["Contacts"];
            IEnumerable<DataRow> query =
                from product in contacts.AsEnumerable()
                where product["Identity"].Equals(identity)
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

        DataRow GetContactByNo(int contactNo)
        {
            DataRow dr = null;
            if (contactNo >= 0)
            {
                _contactsDataView.RowFilter = "contactno='" + contactNo + "'";
                _contactsDataView.Sort = "identity";

                if (_contactsDataView.Count > 0)
                {
                    dr = _contactsDataView[0].Row;
                }
            }
            return dr;
        }

        #endregion
    }
}
