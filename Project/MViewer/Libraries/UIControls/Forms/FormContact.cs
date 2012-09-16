using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;
using GenericData;

namespace UIControls
{
    public partial class FormContact : Form
    {
        #region private members

        FormModes.FormMode _formMode;
        IContactsRepository _repository;

        #endregion

        #region c-tor

        public FormContact(FormModes.FormMode formMode, IContactsRepository repository)
        {
            _formMode = formMode;

            _repository = repository;

            InitializeComponent();

            SetFormMode();
        }

        public FormContact(FormModes.FormMode formMode, IContactsRepository repository, Contact contact)
        {
            _formMode = formMode;
            _repository = repository;

            txtFriendlyName.Text = contact.FriendlyName;
            txtIdentity.Text = contact.Identity;

            InitializeComponent();

            SetFormMode();
        }

        #endregion

        #region private methods

        void SetFormMode()
        {
            switch (_formMode)
            {
                case FormModes.FormMode.Add:
                    btnAdd.Text = "Add";
                    txtIdentity.Enabled = true;
                    break;
                case FormModes.FormMode.Update:
                    btnAdd.Text = "Update";
                    txtIdentity.Enabled = false;
                    break;
            }
        }

        #endregion

        #region callbacks

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // todo: validate input data

            switch (_formMode)
            {
                case FormModes.FormMode.Add:
                    Contact contact = new Contact(txtFriendlyName.Text.Trim(), txtIdentity.Text.Trim());
                    _repository.AddContact(contact);
                    break;
                case FormModes.FormMode.Update:
                    Contact contact2 = new Contact(txtFriendlyName.Text.Trim(), txtIdentity.Text.Trim());
                    _repository.UpdateContact(contact2);
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // todo: implement btnCancel_Click
        }

        #endregion
    }
}
