using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericObjects;
using Utils;
using System.Threading;

namespace MViewer
{
    public partial class FormVideoRoom : Form, IVideoRoom
    {
        #region private members

        bool _formClosing;
        ManualResetEvent _syncClosing = new ManualResetEvent(true);

        public ManualResetEvent SyncClosing
        {
            get { return _syncClosing; }
        }

        #endregion

        #region c-tor

        public FormVideoRoom(string identity)
        {
            InitializeComponent();

            PartnerIdentity = identity;
            _formClosing = false;
        }

        #endregion

        #region callbacks

        private void FormVideoRoom_Activated(object sender, EventArgs e)
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

        private void FormVideoRoom_Resize(object sender, EventArgs e)
        {
            try
            {
                pnlMain.Width = this.Width - 20 - 1;
                pnlMain.Height = this.Height - 20 - 1;

                videoControl.Width = pnlMain.Width - 15 - 5;
                videoControl.Height = pnlMain.Height - 38 - 5;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        private void FormVideoRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _formClosing = true;
                _syncClosing.Set();
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
                Tools.Instance.GenericMethods.ChangeLanguage(language, this.Controls, typeof(FormVideoRoom));
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
                videoControl.SetPartnerName(friendlyName);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void SetPicture(Image picture)
        {
            try
            {
                _syncClosing.WaitOne();
                if (!_formClosing)
                {
                    videoControl.SetPicture(picture);
                }
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

        #endregion

        #region proprieties

        public string PartnerIdentity
        {
            get;
            set;
        }

        public GenericEnums.RoomType RoomType
        {
            get
            {
                return Utils.GenericEnums.RoomType.Video;
            }
        }

        public IntPtr FormHandle
        {
            get
            {
                object value = null;
                Tools.Instance.CrossThreadingControl.GetValue(this, "Handle", ref value);
                return (IntPtr)value;
            }
        }

        #endregion

        private void FormVideoRoom_Deactivate(object sender, EventArgs e)
        { 
            // todo: find a better way to update the button labels
            //Program.Controller.OnActiveRoomChanged(string.Empty, this.RoomType);
        }
    }
}
