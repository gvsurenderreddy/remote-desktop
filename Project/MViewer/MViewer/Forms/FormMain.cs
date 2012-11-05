using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GenericDataLayer;
using UIControls;
using Utils;

namespace MViewer
{
    public partial class FormMain : Form
    { 
        #region private members

        // controller events


        #endregion

        #region observers

        public readonly EventHandlers.IdentityEventHandler IdentityObserver;
        public readonly EventHandlers.ContactsEventHandler ContactsObserver;

        #endregion

        #region c-tor

        public FormMain()
        {
            InitializeComponent();

            IdentityObserver = new EventHandlers.IdentityEventHandler(UpdateIdentity);
            ContactsObserver = new EventHandlers.ContactsEventHandler(ContactsUpdated);
        }

        #endregion

        #region event callbacks

        private void ContactsUpdated(object sender, EventArgs e)
        {
            // todo: implement ContactsUpdated
            if (((ContactsEventArgs)e).Operation == GenericEnums.ContactsOperation.Load && ((ContactsEventArgs)e).ContactsDV != null)
            {
                contactsControl.SetContacts(((ContactsEventArgs)e).ContactsDV);
            }
            else
            {
                Contact contact = Program.Controller.PerformContactsOperation(sender, (ContactsEventArgs)e);
                ((ContactsEventArgs)e).UpdatedContact = contact;
            }

        }

        private void UpdateIdentity(object sender, EventArgs e)
        {
            IdentityEventArgs args = (IdentityEventArgs)e;
            identityControl.UpdateMyID(args.MyIdentity);
            identityControl.UpdateFriendlyName(args.FriendlyName);
        }

        private void IdentityUpdated(object sender, EventArgs e)
        {
            IdentityEventArgs args = (IdentityEventArgs)e;
            // update the identity in the Model by using the Controller
            Program.Controller.IdentityObserver(sender, args);
        }

        private void ContactsControl_ClosePressed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            Program.Controller.StopApplication();
        }

        #endregion

        #region private methods

        

        #endregion

        #region public methods

        public KeyValuePair<string, string> GetSelectedContact()
        {
            return contactsControl.GetSelectedContact();
        }

        #endregion
    }
}
