using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericObjects;
using System.Collections;
using UIControls;
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
        string _language;

        #endregion

        #region c-tor

        public ContactsControl()
        {
            try
            {
                _language = "en-US";
                InitializeComponent();

                InitializeNotificationLabel();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public ContactsControl(EventHandler onClosePressed, EventHandler onContactsUpdated, EventHandler onSelectedContactChanged)
        {
            try
            {
                InitializeComponent();
                InitializeNotificationLabel();
                _onClosePressed = onClosePressed;
                _onContactsUpdated = onContactsUpdated;
                _onSelectedContactChanged = onSelectedContactChanged;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public void ChangeLanguage(string language)
        {
            try
            {
                _language = language;
                Tools.Instance.GenericMethods.ChangeLanguage(language, this.Controls, typeof(ContactsControl));
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SetContacts(DataView dvContacts)
        {
            try
            {
                Thread.Sleep(200);
                if (dvContacts.DataViewManager.DataSet.Tables[0].Rows.Count > 0)
                {
                    Tools.Instance.CrossThreadingControl.SetValue(dgvContacts, true, "Visible");
                    ShowNotification(false);
                    Tools.Instance.CrossThreadingControl.SetValue(dgvContacts, dvContacts, "Datasource");
                    Tools.Instance.CrossThreadingControl.SetGridViewColumnPropery(dgvContacts, "Identity", false, "Visible");
                    Tools.Instance.CrossThreadingControl.SetGridViewColumnPropery(dgvContacts, "ContactNo", false, "Visible");
                    Tools.Instance.CrossThreadingControl.SetGridViewColumnPropery(dgvContacts, "FriendlyName", "Friendly name", "HeaderText");
                    Tools.Instance.CrossThreadingControl.SetGridViewColumnPropery(dgvContacts, "FriendlyName", dgvContacts.Width / 2 - 1, "Width");
                    Tools.Instance.CrossThreadingControl.SetGridViewColumnPropery(dgvContacts, "Status", dgvContacts.Width / 2 - 1, "Width");
                    Tools.Instance.CrossThreadingControl.SetGridViewColumnPropery(dgvContacts, "FriendlyName", DataGridViewTriState.False, "Resizable");
                    Tools.Instance.CrossThreadingControl.SetGridViewColumnPropery(dgvContacts, "Status", DataGridViewTriState.False, "Resizable");
                }
                else
                {
                    Tools.Instance.CrossThreadingControl.SetValue(dgvContacts, false, "Visible");
                    ShowNotification(true);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region event callbacks

        private void ContactsControl_ClosePressed(object sender, EventArgs e)
        {
            try
            {
                if (_onClosePressed != null)
                {
                    _onClosePressed.BeginInvoke(sender, e, null, null);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void FormActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // this form should not be closed while the app is running
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                FormContact formContact = new FormContact(GenericEnums.FormMode.Add, _onContactsUpdated);
                formContact.ChangeLanguage(this._language);
                formContact.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow selectedRow = dgvContacts.SelectedRows[0];
                FormContact formContact = new FormContact(GenericEnums.FormMode.Update, int.Parse(selectedRow.Cells["ContactNo"].Value.ToString()), _onContactsUpdated);
                formContact.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void dgvContacts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region private methods

        private void dgvContacts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Tools.Instance.Logger.LogError(e.Exception.ToString());
        }

        void InitializeNotificationLabel()
        {
            try
            {
                _notification = new Label();
                _notification.Text = "Nobody online";
                _notification.Visible = false;
                pnlContacts.Controls.Add(_notification);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void ShowNotification(bool show)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void SetProperty(bool show)
        {
            try
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public KeyValuePair<string, string> GetSelectedContact()
        {
            KeyValuePair<string, string> contact = new KeyValuePair<string, string>();
            try
            {
                DataGridViewSelectedRowCollection selectedRows = dgvContacts.SelectedRows;
                if (selectedRows.Count > 0)
                {
                    string identity = selectedRows[0].Cells["Identity"].Value.ToString();
                    string friendlyName = selectedRows[0].Cells["FriendlyName"].Value.ToString();
                    contact = new KeyValuePair<string, string>(identity, friendlyName);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            return contact;
        }

        #endregion



    }
}
