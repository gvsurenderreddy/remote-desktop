using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GenericData;

namespace GenericData
{
    public interface IContactsRepository
    {
        DataView LoadContacts();
        void AddContact(Contact contact);
        void RemoveContact(string identity);
        void UpdateContact(Contact contact);
        Contact GetContact(string identity);
    }
}
