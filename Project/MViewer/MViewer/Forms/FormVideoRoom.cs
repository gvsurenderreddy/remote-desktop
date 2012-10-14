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
using UIControls.CrossThreadOperations;
using System.Threading;

namespace MViewer
{
    public partial class FormVideoRoom : Form, IVideoRoom, IRoom
    {
        #region private members

        

        #endregion

        #region c-tor

        public FormVideoRoom(ref IntPtr handle)
        {
            InitializeComponent();
            handle = this.Handle;
        }

        #endregion

        #region private methods

        #endregion

        #region public methods

        public void SetPartnerName(string friendlyName)
        {
            videoControl.SetPartnerName(friendlyName);
        }

        public void SetPicture(Image picture)
        {
            videoControl.SetPicture(picture);
        }

        public void CloseRoom()
        {
            // todo: implement CloseRoom
            this.Close();
        }

        public void ShowRoom()
        {
            // todo: implement ShowRoom
            this.ShowDialog();
        }

        #endregion

        #region proprieties

        public GenericEnums.RoomActionType RoomType
        {
            get
            {
                return Utils.GenericEnums.RoomActionType.Video;
            }
        }

        public IntPtr FormHandle
        {
            get
            {
                object value = null;
                ControlCrossThreading.GetValue(this, "Handle", ref value);
                return (IntPtr)value;
            }
        }

        #endregion
    }
}
