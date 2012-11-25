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
        EventHandler _formClosingEvent;

        public FormMyWebcam(EventHandler formClosingEvent)
        {
            _formClosingEvent = formClosingEvent;
            InitializeComponent();
            _webcamCapture = new WebcamCapture(20, this.Handle.ToInt32(), this.WebcaptureClosing);
            Program.Controller.StartVideoChat(_webcamCapture);
        }

        #region public methods

        public void WebcaptureClosing(object sender, EventArgs args)
        {
            this.Close();
            this.Dispose();
            // todo: notify the View that webcapturing has stopped and it should open a new form next time
            _formClosingEvent.Invoke(null, null);
        }

        public void SetPicture(Image image)
        {
            if (!_webcamCapture.ThreadAborted)
            {
                Image resized = ImageConverter.ResizeImage(image, pbWebcam.Width, pbWebcam.Height);
                pbWebcam.Image = resized;
            }
            else
            {
                _webcamCapture.StopCapturing();
                this.Close();
            }
        }

        public void StopCapturing()
        {
            _webcamCapture.StopCapturing();
        }

        #endregion

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
