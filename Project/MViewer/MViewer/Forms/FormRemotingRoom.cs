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

namespace MViewer.Forms
{
    public partial class FormRemotingRoom : Form, IRemotingRoom
    {
        #region private members

        ManualResetEvent _syncClosing = new ManualResetEvent(true);

        #endregion

        #region c-tor

        public FormRemotingRoom(string identity)
        {
            InitializeComponent();
            ContactIdentity = identity;
        }

        #endregion

        #region public methods

        public void ShowMouseCapture(byte[] capture)
        {
            // todo: implement ShowMouseCapture
        }

        public void ShowScreenCapture(byte[] capture)
        {
            // todo: implement ShowScreenCapture
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
    }
}
