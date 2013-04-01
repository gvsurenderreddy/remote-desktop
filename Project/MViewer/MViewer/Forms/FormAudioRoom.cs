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
    public partial class FormAudioRoom : Form, IAudioRoom
    {
        #region private members

        ManualResetEvent _syncClosing = new ManualResetEvent(true);
        EventHandler _onCaptureReceived;

        #endregion

        #region c-tor

        public FormAudioRoom(string identity, EventHandler onCaptureReceived)
        {
            InitializeComponent();
            PartnerIdentity = identity;
            _onCaptureReceived = onCaptureReceived;
        }

        #endregion

        #region public methods

        public void ChangeLanguage(string language)
        {
            Tools.Instance.GenericMethods.ChangeLanguage(language, this.Controls, typeof(FormAudioRoom));
        }

        public void PlayAudioCapture(byte[] capture)
        {
            _onCaptureReceived.BeginInvoke(this, new AudioCaptureEventArgs()
            {
                Capture = capture,
                Identity = this.PartnerIdentity
            }, null, null);
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

        public void ToggleAudioStatus()
        {
            audioControl.ToggleStatusUpdate();
        }

        #endregion

        #region proprieties

        public GenericEnums.RoomType RoomType
        {
            get { return GenericEnums.RoomType.Audio; }
        }

        public string PartnerIdentity
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
            //Program.Controller.OnActiveRoomChanged(string.Empty, this.RoomType);
        }

        private void FormAudioRoom_Activated(object sender, EventArgs e)
        {
            // tell the controller to update the active form
            Program.Controller.OnActiveRoomChanged(this.PartnerIdentity, this.RoomType);
        }
    }
}
