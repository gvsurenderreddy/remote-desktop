using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace UIControls
{
    public partial class VideoControl : UserControl
    {
        #region c-tor

        public VideoControl()
        {
            InitializeComponent();
        }

        #endregion

        #region public methods

        public void SetPartnerName(string friendlyName)
        {
            try
            {
                txtPartner.Text = friendlyName;
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
                Image resized = Tools.Instance.ImageConverter.ResizeImage(picture, pbVideo.Width, pbVideo.Height);
                pbVideo.Image = resized;
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region callbacks

        private void VideoControl_Resize(object sender, EventArgs e)
        {
            try
            {
                pnlVideo.Width = this.Width - 3 - 5;
                pnlVideo.Height = this.Height - 3 - 5;

                pbVideo.Width = pnlVideo.Width - 15 - 5;
                pbVideo.Height = pnlVideo.Height - 47 - 5;

                pbVideo.Image = Tools.Instance.ImageConverter.ResizeImage(pbVideo.Image, pbVideo.Width, pbVideo.Height);
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
