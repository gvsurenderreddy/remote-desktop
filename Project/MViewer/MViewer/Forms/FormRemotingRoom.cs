using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericDataLayer;
using Utils;

namespace MViewer.Forms
{
    public partial class FormRemotingRoom : Form, IRemotingRoom, IRoom
    {
        #region c-tor

        public FormRemotingRoom()
        {
            InitializeComponent();
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
            // todo: implement PlayAudioCapture
        }

        public void CloseRoom()
        {
            // todo: implement CloseRoom
        }

        public void ShowRoom()
        {
            // todo: implement ShowRoom
        }

        #endregion

        #region proprieties

        public GenericEnums.RoomActionType RoomType
        {
            get { return GenericEnums.RoomActionType.Remoting; }
        }

        #endregion
    }
}
