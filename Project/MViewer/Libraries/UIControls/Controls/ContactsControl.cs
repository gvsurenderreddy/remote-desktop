using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericDataLayer;
using System.Collections;
using UIControls.CrossThreadOperations;
using System.Threading;
using Utils;

namespace UIControls
{
    public partial class ContactsControl : UserControl
    {
        #region private members

        EventHandler _closePressed;
        EventHandler _contactsUpdated;
        Label _notification;

        #endregion

        #region c-tor

        public ContactsControl()
        {
            InitializeComponent();

            InitializeNotificationLabel();
        }

        public ContactsControl(EventHandler closePressed, EventHandler contactsUpdated)
        {
            InitializeComponent();
            InitializeNotificationLabel();
            _closePressed = closePressed;
            _contactsUpdated = contactsUpdated;
        }

        #endregion

        #region public methods

        public void SetContacts(DataView dvContacts)
        {
            Thread.Sleep(1000);
            if (dvContacts.DataViewManager.DataSet.Tables[0].Rows.Count > 0)
            {
                ControlCrossThreading.SetValue(dgvContacts, true, "Visible");
                ShowNotification(false);
                ControlCrossThreading.SetValue(dgvContacts, dvContacts, "Datasource");
                ControlCrossThreading.SetGridViewColumnPropery(dgvContacts, "Identity", false, "Visible");
                ControlCrossThreading.SetGridViewColumnPropery(dgvContacts, "ContactNo", false, "Visible");
                ControlCrossThreading.SetGridViewColumnPropery(dgvContacts, "FriendlyName", "Friendly name", "HeaderText");
                ControlCrossThreading.SetGridViewColumnPropery(dgvContacts, "FriendlyName", dgvContacts.Width - 2, "Width");
            }
            else
            {
                ControlCrossThreading.SetValue(dgvContacts, false, "Visible");
                ShowNotification(true);
            }
        }

        #endregion

        #region event callbacks

        private void ContactsControl_ClosePressed(object sender, EventArgs e)
        {
            if (_closePressed != null)
            {
                _closePressed.Invoke(sender, e);
            }
        }

        private void FormActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            // this form should not be closed while the app is running
            e.Cancel = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormContact formContact = new FormContact(GenericEnums.FormMode.Add, _contactsUpdated);
            formContact.ShowDialog(this);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvContacts.SelectedRows[0];
            Contact contact = new Contact(int.Parse(selectedRow.Cells["ContactNo"].Value.ToString()), string.Empty, string.Empty);
            // todo: pass the removed contact no as argument
            _contactsUpdated.Invoke(this, new ContactsEventArgs()
                {
                    UpdatedContact = contact,
                    Operation = GenericEnums.ContactsOperation.Remove
                });
            _contactsUpdated.Invoke(this, new ContactsEventArgs()
            {
                Operation = GenericEnums.ContactsOperation.Load
            });
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // todo: use the contacts repository only in the Model class
            
            DataGridViewRow selectedRow = dgvContacts.SelectedRows[0];
            
            FormContact formContact = new FormContact(GenericEnums.FormMode.Update, int.Parse(selectedRow.Cells["ContactNo"].Value.ToString()), _contactsUpdated);
            formContact.ShowDialog(this);
        }

        #endregion

        #region private methods

        void InitializeNotificationLabel()
        {
            _notification = new Label();
            _notification.Text = "Nobody online";
            _notification.Visible = false;
            pnlContacts.Controls.Add(_notification);

        }

        void ShowNotification(bool show)
        {
            if (_notification.InvokeRequired)
            {
                SetProperty(show);
            }
            else
            {
                SetProperty(show);
            }
        }

        void SetProperty(bool show)
        {
            _notification.Invoke
            (
                new MethodInvoker
                (
                    delegate
                    {
                        _notification.Visible = show;
                    }
                )
            );
        }


        #endregion
    }
}
