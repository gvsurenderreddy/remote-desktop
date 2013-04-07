using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GenericObjects;
using UIControls;
using Utils;
using Abstraction;

namespace MViewer
{
    public partial class FormMain : Form
    {
        #region private members

        // controller events


        #endregion

        #region observers

        public readonly Delegates.IdentityEventHandler IdentityObserver;
        public readonly Delegates.ContactsEventHandler ContactsObserver;

        #endregion

        #region c-tor

        public FormMain()
        {
            try
            {
                InitializeComponent();

                IdentityObserver = new Delegates.IdentityEventHandler(UpdateIdentity);
                ContactsObserver = new Delegates.ContactsEventHandler(OnContactsUpdated);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region event callbacks

        private void FormMain_Activated(object sender, EventArgs e)
        {
            try
            {
                // update labels according to selected contact
                KeyValuePair<string, string> contact = GetSelectedContact();
                Program.Controller.OnActiveRoomChanged(contact.Key, GenericEnums.RoomType.Audio);
                Program.Controller.OnActiveRoomChanged(contact.Key, GenericEnums.RoomType.Video);
                Program.Controller.OnActiveRoomChanged(contact.Key, GenericEnums.RoomType.Remoting);

                //Program.Controller.FocusActionsForm();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void FormMain_Deactivate(object sender, EventArgs e)
        {
            try
            {
                Program.Controller.OnActiveRoomChanged(string.Empty, GenericEnums.RoomType.Undefined);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void OnSelectedContactChanged(object sender, EventArgs e)
        {
            try
            {
                ContactsEventArgs args = (ContactsEventArgs)e;
                Program.Controller.OnActiveRoomChanged(args.UpdatedContact == null ? string.Empty : args.UpdatedContact.Identity, GenericEnums.RoomType.Audio);
                Program.Controller.OnActiveRoomChanged(args.UpdatedContact == null ? string.Empty : args.UpdatedContact.Identity, GenericEnums.RoomType.Video);
                Program.Controller.OnActiveRoomChanged(args.UpdatedContact == null ? string.Empty : args.UpdatedContact.Identity, GenericEnums.RoomType.Remoting);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void OnContactsUpdated(object sender, EventArgs e)
        {
            try
            {
                if (((ContactsEventArgs)e).Operation == GenericEnums.ContactsOperation.Load && ((ContactsEventArgs)e).ContactsDV != null)
                {
                    contactsControl.SetContacts(((ContactsEventArgs)e).ContactsDV);
                }
                else
                {
                    ContactBase contact = Program.Controller.PerformContactsOperation(sender, (ContactsEventArgs)e);
                    ((ContactsEventArgs)e).UpdatedContact = contact;
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void UpdateIdentity(object sender, EventArgs e)
        {
            try
            {
                IdentityEventArgs args = (IdentityEventArgs)e;
                identityControl.UpdateMyID(args.MyIdentity);
                identityControl.UpdateFriendlyName(args.FriendlyName);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void OnLanguageUpdated(object sender, EventArgs e)
        {
            try
            {
                LanguageEventArgs args = (LanguageEventArgs)e;
                Program.Controller.ChangeLanguage(args.Language);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void OnIdentityUpdated(object sender, EventArgs e)
        {
            try
            {
                IdentityEventArgs args = (IdentityEventArgs)e;
                // update the identity in the Model by using the Controller
                Program.Controller.FriendlyNameObserver(sender, args);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void OnContactsControl_ClosePressed(object sender, EventArgs e)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate() { this.Enabled = false; }));
                Program.Controller.StopApplication();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                e.Cancel = true; //prevent the form from closing if the Exit app confirmation wasn't received
                Program.Controller.StopApplication();
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
                Tools.Instance.GenericMethods.ChangeLanguage(language, this.Controls, typeof(FormMain));
                contactsControl.ChangeLanguage(language);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SetFormMainBackground(string filePath)
        {
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                this.BackgroundImage = Image.FromFile(fileInfo.FullName);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SetMessageText(string text)
        {
            try
            {
                Label lblActionResult = new System.Windows.Forms.Label();
                // 
                lblActionResult.AutoSize = true;
                lblActionResult.Location = new System.Drawing.Point(12, 283);
                lblActionResult.Name = "lblActionResult";
                lblActionResult.Size = new System.Drawing.Size(0, 13);
                lblActionResult.TabIndex = 0;
                lblActionResult.Visible = false;
                {

                    this.Invoke(new MethodInvoker(delegate()
                    {
                        lblActionResult.Text = text;
                        lblActionResult.Visible = true;
                        lblActionResult.Refresh();
                        lblActionResult.Update();
                        this.Refresh();
                        this.Update();
                    }));

                    this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(lblActionResult); }));

                    Thread t = new Thread(delegate()
                        {
                            try
                            {
                                Thread.Sleep(5000);
                                this.Invoke(new MethodInvoker(delegate()
                                    {
                                        this.Controls.Remove(lblActionResult);
                                    }));
                            }
                            catch (Exception ex)
                            {
                                Tools.Instance.Logger.LogError(ex.ToString());
                            }
                        });
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public KeyValuePair<string, string> GetSelectedContact()
        {
            return contactsControl.GetSelectedContact();
        }

        #endregion

    }
}
