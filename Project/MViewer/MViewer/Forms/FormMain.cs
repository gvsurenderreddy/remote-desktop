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

        public readonly Delegates.IdentityEventHandler IdentityObserver;
        public readonly Delegates.ContactsEventHandler ContactsObserver;

        #endregion

        #region c-tor

        public FormMain()
        {
            InitializeComponent();

            IdentityObserver = new Delegates.IdentityEventHandler(UpdateIdentity);
            ContactsObserver = new Delegates.ContactsEventHandler(OnContactsUpdated);
        }

        #endregion

        #region event callbacks

        private void FormMain_Activated(object sender, EventArgs e)
        {
            // update labels according to selected contact
            KeyValuePair<string, string> contact = GetSelectedContact();
            Program.Controller.ActiveRoomChanged(contact.Key, GenericEnums.RoomType.Audio);
            Program.Controller.ActiveRoomChanged(contact.Key, GenericEnums.RoomType.Video);
            Program.Controller.ActiveRoomChanged(contact.Key, GenericEnums.RoomType.Remoting);
        }

        private void FormMain_Deactivate(object sender, EventArgs e)
        {
            Program.Controller.ActiveRoomChanged(string.Empty, GenericEnums.RoomType.Undefined);
        }

        private void OnSelectedContactChanged(object sender, EventArgs e)
        {
            ContactsEventArgs args = (ContactsEventArgs)e;
            Program.Controller.ActiveRoomChanged(args.UpdatedContact == null ? string.Empty : args.UpdatedContact.Identity, GenericEnums.RoomType.Audio);
            Program.Controller.ActiveRoomChanged(args.UpdatedContact == null ? string.Empty : args.UpdatedContact.Identity, GenericEnums.RoomType.Video);
            Program.Controller.ActiveRoomChanged(args.UpdatedContact == null ? string.Empty : args.UpdatedContact.Identity, GenericEnums.RoomType.Remoting);
        }

        private void OnContactsUpdated(object sender, EventArgs e)
        {
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

        private void OnContactsControl_ClosePressed(object sender, EventArgs e)
        {
            Program.Controller.StopApplication();
        }

        private void FormIsClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; //prevent the form from closing if the Exit app confirmation wasn't received
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
