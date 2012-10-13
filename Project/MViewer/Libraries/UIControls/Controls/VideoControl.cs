using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UIControls
{
    public partial class VideoControl : UserControl
    {
        public VideoControl()
        {
            InitializeComponent();
        }

        #region public methods

        public void SetPartnerName(string friendlyName)
        {
            txtPartner.Text = friendlyName;
        }

        public void SetPicture(Image picture)
        {
            pbVideo.Image = picture;
        }

        #endregion
    }
}
