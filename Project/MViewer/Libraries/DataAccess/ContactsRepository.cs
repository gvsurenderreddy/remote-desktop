using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericData;
using System.Data;

namespace DataAccess
{
    public class ContactsRepository
    {
        #region private members

        protected string _xmlFilePath;
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

        protected DataView LoadContacts()
        {
            _contactsDataSet = new DataSet();
            _contactsDataView = new DataView();

            _contactsDataSet.Clear();
            _contactsDataSet.ReadXml(_xmlFilePath, XmlReadMode.ReadSchema);
            _contactsDataView = _contactsDataSet.Tables[0].DefaultView;
            return _contactsDataView;
        }

        protected void AddContact(Contact contact)
        {
            DataRow dr = _contactsDataView.Table.NewRow();
            dr[0] = contact.FriendlyName;
            dr[1] = contact.Identity;
            _contactsDataView.Table.Rows.Add(dr);
            SaveContacts();
        }

        protected void RemoveContact(string identity)
        {
            _contactsDataView.RowFilter = "identity='" + identity + "'";
            _contactsDataView.Sort = "identity";
            _contactsDataView.Delete(0);
            _contactsDataView.RowFilter = "";
            SaveContacts();
        }

        protected void UpdateContact(Contact contact)
        {
            Contact dr = GetContact(contact.Identity);
            dr.FriendlyName = contact.FriendlyName;
            SaveContacts();
        }

        protected Contact GetContact(string identity)
        {
            _contactsDataView.RowFilter = "identity='" + identity + "'";
            _contactsDataView.Sort = "identity";
            DataRow dr = null;
            if (_contactsDataView.Count > 0)
            {
                dr = _contactsDataView[0].Row;
            }
            _contactsDataView.RowFilter = "";
            Contact contact = new Contact(dr[0].ToString(), dr[1].ToString());
            return contact;
        }

        protected void SaveContacts()
        {
            _contactsDataSet.WriteXml(_xmlFilePath, XmlWriteMode.WriteSchema);
        }

        #endregion

    }
}
