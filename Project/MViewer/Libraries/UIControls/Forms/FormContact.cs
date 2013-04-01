using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;
using GenericObjects;
using System.Threading;

namespace UIControls
{
    public partial class FormContact : Form
    {
        #region private members

        GenericEnums.FormMode _formMode;
        EventHandler _contactsUpdated;
        int _contactNo;

        #endregion

        #region c-tor

        public FormContact(GenericEnums.FormMode formMode, EventHandler contactsUpdated)
        {
            _formMode = formMode;
            _contactsUpdated = contactsUpdated;

            InitializeComponent();

            SetFormMode();

        }

        public FormContact(GenericEnums.FormMode formMode, int contactNo, EventHandler contactsUpdated)
        {
            _formMode = formMode;
            _contactsUpdated = contactsUpdated;
            _contactNo = contactNo;

            InitializeComponent();
            Contact contact = new Contact(contactNo, string.Empty, string.Empty);
            ContactsEventArgs eventArgs = new ContactsEventArgs()
                {
                    Operation = GenericEnums.ContactsOperation.Get,
                    UpdatedContact = contact
                };
            contactsUpdated.Invoke(this, eventArgs);
            // retrieve contact info
            txtFriendlyName.Text = eventArgs.UpdatedContact.FriendlyName;
            txtIdentity.Text = eventArgs.UpdatedContact.Identity;

            SetFormMode();

        }

        #endregion

        #region public methods

        // todo: use ChangeLanguage from FormContact
        public void ChangeLanguage(string language)
        {
            Tools.Instance.GenericMethods.ChangeLanguage(language, this.Controls, typeof(FormContact));
        }

        #endregion

        #region private methods

        void SetFormMode()
        {
            switch (_formMode)
            {
                case GenericEnums.FormMode.Add:
                    btnAdd.Text = "Add";
                    txtIdentity.Enabled = true;
                    break;
                case GenericEnums.FormMode.Update:
                    btnAdd.Text = "Update";
                    txtIdentity.Enabled = false;
                    break;
            }
        }

        #endregion

        #region callbacks

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // validate input data

            if(string.IsNullOrEmpty(txtFriendlyName.Text.Trim()) || string.IsNullOrEmpty(txtIdentity.Text.Trim()))
            {
                MessageBox.Show("Cannot insert empty text");
                return;
            }

            switch (_formMode)
            {
                case GenericEnums.FormMode.Add:
                    Contact contact = new Contact(0, txtFriendlyName.Text.Trim(), txtIdentity.Text.Trim());
                    _contactsUpdated.BeginInvoke(sender, new ContactsEventArgs
                    {
                        UpdatedContact = contact,
                        Operation = GenericEnums.ContactsOperation.Add
                    }, null, null);

                    break;
                case GenericEnums.FormMode.Update:
                    Contact contact2 = new Contact(_contactNo, txtFriendlyName.Text.Trim(), txtIdentity.Text.Trim());
                    _contactsUpdated.Invoke(sender, new ContactsEventArgs
                    {
                        UpdatedContact = contact2,
                        Operation = GenericEnums.ContactsOperation.Update
                    });

                    break;
            }
            _contactsUpdated.BeginInvoke(this, new ContactsEventArgs()
                {
                    Operation = GenericEnums.ContactsOperation.Load
                }, null, null);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
