using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GenericDataLayer;
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

        public void ShowMouseCapture(byte[] capture)
        {
            // todo: implement ShowMouseCapture
        }

        public void ShowScreenCapture(byte[] capture)
        {
            _syncClosing.WaitOne();
            Image picture = ImageConverter.ByteArrayToImage(capture);
            if (!_formClosing)
            {
                remoteControl.SetPicture(picture);
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

            // todo: later - perform other specific actions when the remoting Chat room is closing
            _syncClosing.Set();
        }

        private void FormRemotingRoom_Resize(object sender, EventArgs e)
        {
            pnlMain.Width = this.Width - 20 - 1;
            pnlMain.Height = this.Height - 20 - 1;

            remoteControl.Width = pnlMain.Width - 15 - 5;
            remoteControl.Height = pnlMain.Height - 38 - 5;
        }

        #endregion
    }
}
