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
            Image resized = ImageConverter.ResizeImage(picture, pbVideo.Width, pbVideo.Height);
            pbVideo.Image = resized;
        }

        #endregion

        #region callbacks

        private void VideoControl_Resize(object sender, EventArgs e)
        {
            pnlVideo.Width = this.Width - 3 - 5;
            pnlVideo.Height = this.Height - 3 - 5;

            pbVideo.Width = pnlVideo.Width - 15 - 5;
            pbVideo.Height = pnlVideo.Height - 47 - 5;

            pbVideo.Image = ImageConverter.ResizeImage(pbVideo.Image, pbVideo.Width, pbVideo.Height);
        }

        #endregion
    }
}
