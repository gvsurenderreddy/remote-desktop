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
            try
            {
                InitializeComponent();
                PartnerIdentity = identity;
                _onCaptureReceived = onCaptureReceived;
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
            Tools.Instance.GenericMethods.ChangeLanguage(language, this.Controls, typeof(FormAudioRoom));
        }

        public void PlayAudioCapture(byte[] capture, double captureLengthInSeconds)
        {
            try
            {
                _onCaptureReceived.Invoke(this, new AudioCaptureEventArgs()
                {
                    Capture = capture,
                    Identity = this.PartnerIdentity,
                    CaptureLengthInSeconds = captureLengthInSeconds
                });
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SetPartnerName(string friendlyName)
        {
            try
            {
                audioControl.SetPartnerName(friendlyName);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
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
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void ToggleAudioStatus()
        {
            try
            {
                audioControl.ToggleStatusUpdate();
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
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
            // todo: optional - find a better way to update the button labels
            //Program.Controller.OnActiveRoomChanged(string.Empty, this.RoomType);
        }

        private void FormAudioRoom_Activated(object sender, EventArgs e)
        {
            try
            {
                // tell the controller to update the active form
                Program.Controller.OnActiveRoomChanged(this.PartnerIdentity, this.RoomType);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }
    }
}
