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
            // todo: implement SetPartnerName - audio room
        }

        public void CloseRoom()
        {
            // todo: implement CloseRoom - audio room
        }

        public void ShowRoom()
        {
            // todo: implement ShowRoom - audio room
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
    }
}
