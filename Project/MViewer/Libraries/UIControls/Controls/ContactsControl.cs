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

        EventHandler _onClosePressed;
        EventHandler _onContactsUpdated;
        EventHandler _onSelectedContactChanged;
        Label _notification;

        #endregion

        #region c-tor

        public ContactsControl()
        {
            InitializeComponent();

            InitializeNotificationLabel();
        }

        public ContactsControl(EventHandler onClosePressed, EventHandler onContactsUpdated, EventHandler onSelectedContactChanged)
        {
            InitializeComponent();
            InitializeNotificationLabel();
            _onClosePressed = onClosePressed;
            _onContactsUpdated = onContactsUpdated;
            _onSelectedContactChanged = onSelectedContactChanged;
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
                ControlCrossThreading.SetGridViewColumnPropery(dgvContacts, "FriendlyName", dgvContacts.Width / 2 - 1, "Width");
                ControlCrossThreading.SetGridViewColumnPropery(dgvContacts, "Status", dgvContacts.Width / 2 - 1, "Width");

                ControlCrossThreading.SetGridViewColumnPropery(dgvContacts, "FriendlyName", DataGridViewTriState.False, "Resizable");
                ControlCrossThreading.SetGridViewColumnPropery(dgvContacts, "Status", DataGridViewTriState.False, "Resizable");
                
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
            if (_onClosePressed != null)
            {
                _onClosePressed.Invoke(sender, e);
            }
        }

        private void FormActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            // this form should not be closed while the app is running
            e.Cancel = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormContact formContact = new FormContact(GenericEnums.FormMode.Add, _onContactsUpdated);
            formContact.ShowDialog(this);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvContacts.SelectedRows[0];
            Contact contact = new Contact(int.Parse(selectedRow.Cells["ContactNo"].Value.ToString()),
                selectedRow.Cells["FriendlyName"].Value.ToString(),
                selectedRow.Cells["Identity"].Value.ToString());
            // pass the removed contact no as argument
            _onContactsUpdated.Invoke(this, new ContactsEventArgs()
                {
                    UpdatedContact = contact,
                    Operation = GenericEnums.ContactsOperation.Remove
                });
            _onContactsUpdated.BeginInvoke(this, 
                new ContactsEventArgs()
                {
                    Operation = GenericEnums.ContactsOperation.Load
                },
                null, null
                );
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvContacts.SelectedRows[0];

            FormContact formContact = new FormContact(GenericEnums.FormMode.Update, int.Parse(selectedRow.Cells["ContactNo"].Value.ToString()), _onContactsUpdated);
            formContact.ShowDialog(this);
        }

        private void dgvContacts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvContacts.SelectedRows != null && dgvContacts.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvContacts.SelectedRows[0];
                try
                {
                    Contact contact = new Contact(int.Parse(selectedRow.Cells["ContactNo"].Value.ToString()),
                        selectedRow.Cells["FriendlyName"].Value.ToString(),
                        selectedRow.Cells["Identity"].Value.ToString());
                    _onSelectedContactChanged.Invoke(this,
                        new ContactsEventArgs()
                        {
                            UpdatedContact = contact
                        });
                }
                catch
                {
                    _onSelectedContactChanged.Invoke(this,
                          new ContactsEventArgs()
                          {
                              UpdatedContact = null
                          });
                }
            }
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

        #region public methods

        public KeyValuePair<string, string> GetSelectedContact()
        {
            KeyValuePair<string, string> contact = new KeyValuePair<string, string>();
            DataGridViewSelectedRowCollection selectedRows = dgvContacts.SelectedRows;
            if (selectedRows.Count > 0)
            {
                string identity = selectedRows[0].Cells["Identity"].Value.ToString();
                string friendlyName = selectedRows[0].Cells["FriendlyName"].Value.ToString();
                contact = new KeyValuePair<string, string>(identity, friendlyName);
            }
            return contact;
        }

        #endregion

    }
}
