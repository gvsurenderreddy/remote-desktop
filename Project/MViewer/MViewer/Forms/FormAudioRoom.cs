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
    public partial class FormAudioRoom : Form, IAudioRoom, IRoom
    {
        #region private members

        #endregion

        #region c-tor

        public FormAudioRoom()
        {
            InitializeComponent();
        }

        #endregion

        #region public methods

        public void PlayAudioCapture(byte[] capture)
        {
            // todo: implement PlayAudioCapture
        }

        public void SetPartnerName(string friendlyName)
        {
            // todo: implement SetPartnerName
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
            get { return GenericEnums.RoomActionType.Audio; }
        }

        #endregion
    }
}
