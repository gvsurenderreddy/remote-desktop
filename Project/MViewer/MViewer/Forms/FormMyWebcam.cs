using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericDataLayer;

namespace MViewer
{
    public partial class FormMyWebcam : Form
    {
        WebcamCapture _webcamCapture;

        public FormMyWebcam()
        {
            InitializeComponent();
            _webcamCapture = new WebcamCapture(20, this.Handle.ToInt32());
            Program.Controller.StartVideoChat(_webcamCapture);
        }

        public void SetPicture(Image image)
        {
            Image resized = ImageConverter.ResizeImage(image, pbWebcam.Width, pbWebcam.Height);
            pbWebcam.Image = resized;
        }

        #region prorieties

        public WebcamCapture CaptureControl
        {
            get
            {
                return _webcamCapture;
            }
        }

        #endregion

        #region callbacks

        private void FormMyWebcam_Resize(object sender, EventArgs e)
        {
            pnlMain.Width = this.Width - 42 - 3;
            pnlMain.Height = this.Height - 61 - 5;

            pbWebcam.Width = pnlMain.Width - 22;
            pbWebcam.Height = pnlMain.Height - 22;

            pbWebcam.Image = ImageConverter.ResizeImage(pbWebcam.Image, pbWebcam.Width, pbWebcam.Height);
        }

        #endregion
    }
}
