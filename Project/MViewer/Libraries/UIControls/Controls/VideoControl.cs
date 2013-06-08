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
        readonly object _syncPictures = new object();
        IDictionary<DateTime, Image> _captures;

        #region c-tor

        public VideoControl()
        {
            _captures = new Dictionary<DateTime, Image>();
            InitializeComponent();
        }

        #endregion

        #region private methods

        void AddPicture(Image toAdd)
        {
            _captures.Add(DateTime.Now, toAdd);
        }

        Image PickOldestPicture()
        {
            return _captures[_captures.Keys.Min()];
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
            lock (_syncPictures)
            {
                try
                {
                    Image toDisplay = picture;
                    if (_captures.Count > 0)
                    {
                        toDisplay = PickOldestPicture();
                        this.AddPicture(picture);
                    }
                    if (pbVideo.Width > 0 && pbVideo.Height > 0)
                    {
                        Image resized = Tools.Instance.ImageConverter.ResizeImage(toDisplay, pbVideo.Width, pbVideo.Height);
                        pbVideo.Image = resized;
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            pbVideo.Update();
                            pbVideo.Refresh();
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Tools.Instance.Logger.LogError(ex.ToString());
                }
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
                if (pbVideo.Width > 0 && pbVideo.Height > 0)
                {
                    pbVideo.Image = Tools.Instance.ImageConverter.ResizeImage(pbVideo.Image, pbVideo.Width, pbVideo.Height);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
