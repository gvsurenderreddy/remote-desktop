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
    public partial class FormRemotingRoom : Form, IRemotingRoom
    {
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
            // todo: implement SetPartnerName  - remoting room
        }

        public void CloseRoom()
        {
            // todo: implement CloseRoom - remoting room
        }

        public void ShowRoom()
        {
            // todo: implement ShowRoom - remoting room
        }

        #endregion

        #region proprieties

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
