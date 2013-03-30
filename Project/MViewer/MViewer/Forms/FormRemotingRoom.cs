using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GenericObjects;
using Utils;

namespace MViewer
{
    public partial class FormRemotingRoom : Form, IRemotingRoom
    {
        #region private members

        bool _formClosing;
        ManualResetEvent _syncClosing = new ManualResetEvent(true);

        #endregion

        #region c-tor

        public FormRemotingRoom(string identity)
        {
            InitializeComponent();
            ContactIdentity = identity;
            _formClosing = false;

        }

        #endregion

        #region public methods

        public void BindCommandHandlers(Delegates.HookCommandDelegate remotingCommand)
        {
            remoteControl.BindCommandHandler(remotingCommand);
        }

        public void ShowScreenCapture(byte[] screenCapture, byte[] mouseCapture)
        {
            _syncClosing.WaitOne();
            if (!_formClosing)
            {
                remoteControl.UpdateScreen(screenCapture, mouseCapture);
            }
        }

        public void SetPartnerName(string friendlyName)
        {
            remoteControl.SetPartnerName(friendlyName);
        }

        public void ShowRoom()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke
                        (
                        new MethodInvoker
                        (
                       delegate
                       {
                           this.Show();
                       }
                        )
                        );

                }
                else
                {
                    this.Show();
                }
            }
            catch
            {
            }
        }

        #endregion

        #region proprieties

        public ManualResetEvent SyncClosing
        {
            get { return _syncClosing; }
        }

        public string ContactIdentity
        {
            get;
            set;
        }

        public GenericEnums.RoomType RoomType
        {
            get { return GenericEnums.RoomType.Remoting; }
        }

        #endregion

        #region private methods

        private void FormRemotingRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formClosing = true;

            // todo: later - perform other specific actions when the remoting  room is closing
            _syncClosing.Set();
        }

        private void FormRemotingRoom_Activated(object sender, EventArgs e)
        {
            // tell the controller to update the active form
            Program.Controller.OnActiveRoomChanged(this.ContactIdentity, this.RoomType);

            remoteControl.WireUpEventProvider();
        }

        private void FormRemotingRoom_Resize(object sender, EventArgs e)
        {
            //pnl main width = form width - (591 - 573) = form width - 18
            //pnl main height = form height - (483 - 442) = form height - 41

            //remote control width = pnl main width - (573 - 547) = pnl main width - 26
            //remote control height = pnl main height - (442 - 416) = pnl main height - 26

            pnlMain.Width = this.Width - 18;
            pnlMain.Height = this.Height - 41;

            remoteControl.Width = pnlMain.Width - 26;
            remoteControl.Height = pnlMain.Height - 26;
        }
        private void FormRemotingRoom_Load(object sender, EventArgs e)
        {
            //force the form to double buffer repaints to reduce flickering
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private void FormRemotingRoom_Deactivate(object sender, EventArgs e)
        {
            // todo: find a better way to update the button labels
            remoteControl.WireDownEventProvider();
            Program.Controller.OnActiveRoomChanged(string.Empty, this.RoomType);
        }
        #endregion


    }
}
