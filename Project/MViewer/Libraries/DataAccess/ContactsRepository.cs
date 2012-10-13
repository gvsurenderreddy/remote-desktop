using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericDataLayer;
using System.Data;
using System.IO;

namespace DataAccessLayer
{
    public static class ContactsRepository
    {
        #region private static members

        static string _xmlFilePath;
        static DataSet _contactsDataSet = new DataSet();
        static DataView _contactsDataView = new DataView();

        #endregion

        #region public static methods

        public static DataView LoadContacts(string xmlFilePath)
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
                dtContacts.Columns.Add(new DataColumn("ConnectedPeers", typeof(string)));
                
                _contactsDataSet.Tables.Add(dtContacts);
                _contactsDataView = _contactsDataSet.Tables[0].DefaultView;
            }
            return _contactsDataView;
        }

        public static int AddContact(Contact contact)
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

        public static void RemoveContact(int contactNo)
        {
            _contactsDataView.RowFilter = "ContactNo='" + contactNo + "'";
            _contactsDataView.Sort = "ContactNo";
            _contactsDataView.Delete(0);
            _contactsDataView.RowFilter = "";
            SaveContacts();
            LoadContacts(_xmlFilePath);
        }

        public static void UpdateContact(Contact contact)
        {
            DataRow dr = GetContact2(contact.ContactNo);
            dr.SetField("FriendlyName", contact.FriendlyName);
            SaveContacts(); 
            LoadContacts(_xmlFilePath);
        }

        public static Contact GetContact(int contactNo)
        {
            _contactsDataView.RowFilter = "contactno='" + contactNo + "'";
            _contactsDataView.Sort = "identity";
            DataRow dr = null;
            if (_contactsDataView.Count > 0)
            {
                dr = _contactsDataView[0].Row;
            }
            _contactsDataView.RowFilter = "";

            Contact contact = new Contact(int.Parse(dr["ContactNo"].ToString()), dr["FriendlyName"].ToString(), dr["Identity"].ToString());
            return contact;
        }

        public static void SaveContacts()
        {
            _contactsDataSet.WriteXml(_xmlFilePath, XmlWriteMode.WriteSchema);
        }

        #endregion

        #region private static methods

        static DataRow GetContact2(int contactNo)
        {
            _contactsDataView.RowFilter = "contactno='" + contactNo + "'";
            _contactsDataView.Sort = "identity";
            DataRow dr = null;
            if (_contactsDataView.Count > 0)
            {
                dr = _contactsDataView[0].Row;
            }
            _contactsDataView.RowFilter = "";

            return dr;
        }

        #endregion
    }
}
