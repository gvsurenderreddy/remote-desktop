using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GenericData;
using UIControls;

namespace MViewer
{
    public partial class FormMain : Form
    { 
        #region private members

        

        #endregion

        #region observers

        public readonly EventHandlers.IdentityEventHandler IdentityObserver;
        readonly EventHandlers.IdentityEventHandler _identityObserver2;


        #endregion

        #region c-tor

        public FormMain(EventHandlers.IdentityEventHandler modelUpdater)
        {
            InitializeComponent();

            IdentityObserver = new EventHandlers.IdentityEventHandler(UpdateIdentity);
            _identityObserver2 = modelUpdater;
        }

        #endregion

        #region event callbacks

        private void UpdateIdentity(object sender, EventArgs e)
        {
            IdentityEventArgs args = (IdentityEventArgs)e;
            identityControl.UpdateMyID(args.MyIdentity);
            identityControl.UpdateFriendlyName(args.FriendlyName);
        }

        private void IdentityUpdated(object sender, EventArgs e)
        {
            IdentityEventArgs args = (IdentityEventArgs)e;
            // update the identity in the Model
            _identityObserver2.Invoke(this, args);
        }

        private void ContactsControl_Load(object sender, EventArgs e)
        {
            // todo: load saved contacts
        }

        private void ContactsControl_ClosePressed(object sender, EventArgs e)
        {
            // todo: perform other specific actions for closing application

            Environment.Exit(0);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ContactsControl_ClosePressed(sender, e);
        }

        #endregion

        #region private methods

        

        #endregion

        #region public methods

        public void NotifyIdentityObserver(object sender, IdentityEventArgs e)
        {
            IdentityObserver.Invoke(sender, e);
        }

        #endregion
    }
}
