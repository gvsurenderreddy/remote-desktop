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
    public partial class FormAudioRoom : Form, IAudioRoom
    {
        #region private members
        
        ManualResetEvent _syncClosing = new ManualResetEvent(true);

        #endregion

        #region c-tor

        public FormAudioRoom(string identity)
        {
            InitializeComponent();
            ContactIdentity = identity;
        }

        #endregion

        #region public methods

        public void PlayAudioCapture(byte[] capture)
        {
            // todo: implement PlayAudioCapture - audio room
        }

        public void SetPartnerName(string friendlyName)
        {
            audioControl.SetPartnerName(friendlyName);
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

        public GenericEnums.RoomType RoomType
        {
            get { return GenericEnums.RoomType.Audio; }
        }

        public string ContactIdentity
        {
            get;
            set;
        }

        public ManualResetEvent SyncClosing
        {
            get { return _syncClosing; }
        }

        #endregion

        private void FormAudioRoom_Deactivate(object sender, EventArgs e)
        {
            // todo: find a better way to update the button labels
            //Program.Controller.ActiveRoomChanged(this.ContactIdentity, this.RoomType);
        }

        private void FormAudioRoom_Activated(object sender, EventArgs e)
        {
            // tell the controller to update the active form
            Program.Controller.ActiveRoomChanged(string.Empty, this.RoomType);
        }
    }
}
