using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Abstraction
{
    public interface IContactsDAL
    {
        string[] GetContactIdentities();
        DataSet LoadContacts(string xmlFilePath);
        int AddContact(ContactBase contact);
        void RemoveContact(int contactNo);
        void UpdateContact(ContactBase contact);
        ContactBase GetContactByNumber(int contactNo);
        ContactBase GetContactByIdentity(string identity);
        void SaveContacts();
        int GetContactsCount();
    }
}
